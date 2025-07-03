using System.Net;
using InscripcionUniAPI.Data;
using InscripcionUniAPI.Security;
using InscripcionUniAPI.Services.Implementations;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ---------- Services ----------
builder.Services.AddDbContext<UniversityDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlRailway"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlRailway"))));

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IStudentService,  StudentService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();
builder.Services.AddScoped<ICourseService,   CourseService>();
builder.Services.AddScoped<JwtService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwt.Issuer,
            ValidAudience            = jwt.Audience,
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(o => o.AddPolicy("AllowAll",
    p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// ---------- Forwarded Headers (para X‑Forwarded‑For) ----------
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Con Railway u otros proxies no conocemos de antemano sus IPs, así que
    // limpiamos listas para confiar en el header que llega:
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "InscripcionUniAPI", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Scheme       = "bearer",
        BearerFormat = "JWT",
        Type         = SecuritySchemeType.Http,
        In           = ParameterLocation.Header,
        Description  = "Ingrese: Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ---------- Pipeline ----------

// Acepta X‑Forwarded‑For / Proto antes de todo
app.UseForwardedHeaders();

// Middleware de restricción por IP
app.Use(async (context, next) =>
{
    // 1) Intenta obtener la IP real del header X‑Forwarded‑For
    string? headerIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    IPAddress? clientIp = null;

    if (!string.IsNullOrWhiteSpace(headerIp))
    {
        // El header puede traer varias IP separadas por coma; tomamos la primera
        var first = headerIp.Split(',')[0].Trim();
        IPAddress.TryParse(first, out clientIp);
    }

    // 2) Si no hay header, usa la conexión remota
    clientIp ??= context.Connection.RemoteIpAddress;

    // 3) Normaliza IPv4‑map to IPv6
    if (clientIp is { IsIPv4MappedToIPv6: true })
        clientIp = clientIp.MapToIPv4();

    Console.WriteLine($"IP cliente detectada: {clientIp}");

    // 4) IPs permitidas
    var allowedSchoolIp = IPAddress.Parse("187.155.101.200");
    var localhostV4     = IPAddress.Parse("127.0.0.1");
    var localhostV6     = IPAddress.Parse("::1");

    if (clientIp == null ||
        !(clientIp.Equals(allowedSchoolIp) || clientIp.Equals(localhostV4) || clientIp.Equals(localhostV6)))
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync("Acceso denegado: IP no autorizada.");
        return;
    }

    await next();
});

app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ---------- DB migrate + seed ----------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UniversityDbContext>();
    await db.Database.MigrateAsync();
    await DataSeeder.SeedAsync(db);
}

app.Run();
