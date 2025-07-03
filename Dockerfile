# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar todo
COPY . .

# Restaurar paquetes
RUN dotnet restore InscripcionUniAPI.csproj

# Publicar proyecto
RUN dotnet publish InscripcionUniAPI.csproj -c Release -o /app/out

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Exponer puerto (ajusta según tu API)
EXPOSE 5000

# Iniciar app
ENTRYPOINT ["dotnet", "InscripcionUniAPI.dll"]
