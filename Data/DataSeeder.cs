using Bogus;
using InscripcionUniAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InscripcionUniAPI.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(UniversityDbContext db)
        {
            // Evitar repetir datos si ya hay estudiantes o cursos
            if (await db.Students.AnyAsync() || await db.Courses.AnyAsync())
                return;

            using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                /* ---------- 1. Estudiantes ---------- */
                var students = new Faker<Student>("es")
                    .RuleFor(s => s.Matriculation, f => f.Random.AlphaNumeric(8).ToUpper())
                    .RuleFor(s => s.FirstName,  f => f.Name.FirstName())
                    .RuleFor(s => s.LastName,   f => f.Name.LastName())
                    .RuleFor(s => s.Email,      (f, s) => f.Internet.Email(s.FirstName, s.LastName))
                    .Generate(100);

                await db.Students.AddRangeAsync(students);

                /* ---------- 2. Cursos fijos ---------- */
                var courses = new List<Course>
                {
                    new() { Code = "MAT101",  Name = "Calculus I",          CreditHours = 4 },
                    new() { Code = "PHY101",  Name = "Physics I",           CreditHours = 4 },
                    new() { Code = "CHEM100", Name = "General Chemistry",   CreditHours = 3 },
                    new() { Code = "ENG101",  Name = "English Composition", CreditHours = 3 },
                    new() { Code = "CS101",   Name = "Intro to Programming",CreditHours = 4 },
                    new() { Code = "HIST201", Name = "World History",       CreditHours = 3 },
                    new() { Code = "BIO101",  Name = "Biology I",           CreditHours = 4 },
                    new() { Code = "PHIL100", Name = "Intro to Philosophy", CreditHours = 2 },
                    new() { Code = "ECON101", Name = "Economics I",         CreditHours = 3 },
                    new() { Code = "ART100",  Name = "Art Appreciation",    CreditHours = 2 }
                };

                await db.Courses.AddRangeAsync(courses);

                // Guardar estudiantes y cursos para que tengan Ids
                await db.SaveChangesAsync();

                /* ---------- 3. Semestres e inscripciones ---------- */
                var terms   = new[] { "Primavera", "Otoño" };
                var random  = new Random();

                var semesterEnrollments = new List<SemesterEnrollment>();
                var enrolledCourses     = new List<EnrolledCourse>();

                foreach (var student in students)
                {
                    int semestersCount = random.Next(1, 3);          // 1‑2 semestres

                    for (int i = 0; i < semestersCount; i++)
                    {
                        var semester = new SemesterEnrollment
                        {
                            StudentId       = student.Id,
                            Year            = (ushort)(2023 + random.Next(0, 3)), // 2023‑2025
                            Term            = terms[random.Next(terms.Length)],
                            MaxCreditHours  = 21,
                            Courses         = new List<EnrolledCourse>()
                        };

                        semesterEnrollments.Add(semester);

                        // 2‑4 cursos por semestre
                        int coursesCount = random.Next(2, 5);
                        var selected     = courses.OrderBy(_ => random.Next())
                                                  .Take(coursesCount);

                        foreach (var course in selected)
                        {
                            var enrolled = new EnrolledCourse
                            {
                                CourseId              = course.Id,
                                CreditHours           = course.CreditHours,
                                SemesterEnrollment    = semester
                            };

                            semester.Courses.Add(enrolled);
                            enrolledCourses.Add(enrolled);
                        }
                    }
                }

                await db.SemesterEnrollments.AddRangeAsync(semesterEnrollments);
                await db.EnrolledCourses     .AddRangeAsync(enrolledCourses);

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
