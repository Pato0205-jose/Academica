using Bogus;
using InscripcionUniAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InscripcionUniAPI.Data
{
    public static class DataSeeder
    {
        // Método para limpiar datos (opcional)
        public static async Task ClearDataAsync(UniversityDbContext db)
        {
            // Elimina datos de las tablas en orden correcto (dependencias)
            await db.Database.ExecuteSqlRawAsync("DELETE FROM Students");
            await db.Database.ExecuteSqlRawAsync("DELETE FROM Courses");
            await db.SaveChangesAsync();
        }

        public static async Task SeedAsync(UniversityDbContext db)
        {
            // No continuar si ya existen datos
            if (await db.Students.AnyAsync() || await db.Courses.AnyAsync()) return;

            using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                // Generar y agregar estudiantes falsos
                var students = new Faker<Student>("es")
                    .RuleFor(s => s.Matriculation, f => f.Random.AlphaNumeric(8).ToUpper())
                    .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                    .RuleFor(s => s.LastName, f => f.Name.LastName())
                    .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.FirstName, s.LastName))
                    .Generate(100);

                await db.Students.AddRangeAsync(students);

                // Lista fija de cursos
                var courses = new List<Course>
                {
                    new Course { Code = "MAT101", Name = "Calculus I", CreditHours = 4 },
                    new Course { Code = "PHY101", Name = "Physics I", CreditHours = 4 },
                    new Course { Code = "CHEM100", Name = "General Chemistry", CreditHours = 3 },
                    new Course { Code = "ENG101", Name = "English Composition", CreditHours = 3 },
                    new Course { Code = "CS101", Name = "Intro to Programming", CreditHours = 4 },
                    new Course { Code = "HIST201", Name = "World History", CreditHours = 3 },
                    new Course { Code = "BIO101", Name = "Biology I", CreditHours = 4 },
                    new Course { Code = "PHIL100", Name = "Intro to Philosophy", CreditHours = 2 },
                    new Course { Code = "ECON101", Name = "Economics I", CreditHours = 3 },
                    new Course { Code = "ART100", Name = "Art Appreciation", CreditHours = 2 }
                };

                await db.Courses.AddRangeAsync(courses);

                // Guardar cambios y confirmar transacción
                await db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
