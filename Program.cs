using InscripcionUniAPI.Data;
using InscripcionUniAPI.Security;
using InscripcionUniAPI.Services.Implementations;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
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

// ---------- Middleware para restringir IP ----------
app.Use(async (context, next) =>
{
    var remoteIp = context.Connection.RemoteIpAddress;

    Console.WriteLine($"IP remota detectada: {remoteIp}");

    var allowedIp = IPAddress.Parse("187.155.101.200");

    if (remoteIp != null && remoteIp.IsIPv4MappedToIPv6)
    {
        remoteIp = remoteIp.MapToIPv4();
    }

    if (!remoteIp?.Equals(allowedIp) ?? true)
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
