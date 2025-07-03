# InscripcionUniAPI

API ASP.NET Core 9.0 para gestionar la inscripción académica universitaria.

## Pasos rápidos

1. **Restaurar dependencias**

   ```bash
   dotnet restore
   ```

2. **Crear la base de datos**

   Ejecuta el script `create_university_schema.sql` en tu instancia MySQL
   (Railway). Asegúrate de copiar el *connection string* resultante en
   `appsettings.json`.

3. **Ajustar configuración**

   * `appsettings.json`  
     *Reemplaza* `HOST`, `PORT`, `USER`, `PASSWORD` y la clave JWT.

4. **Compilar y correr**

   ```bash
   dotnet run
   ```

   Abre <http://localhost:5000/swagger> para probar los endpoints.

## Principales carpetas

| Carpeta | Propósito |
| ------- | ---------- |
| **Controllers** | Endpoints HTTP (Students, Semesters, Courses) |
| **Services** | Lógica de aplicación y acceso a datos |
| **Core** | Entidades de dominio y reglas de negocio |
| **Data** | `UniversityDbContext` y `DataSeeder` |
| **Security** | Generación y validación de JWT |

Incluye paginación, reglas de negocio (créditos, unicidad, protección de
borrado), seed de 100 estudiantes + 10 cursos y Swagger con esquema Bearer.
