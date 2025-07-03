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
                // 1. Crear estudiantes falsos
                var students = new Faker<Student>("es")
                    .RuleFor(s => s.Matriculation, f => f.Random.AlphaNumeric(8).ToUpper())
                    .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                    .RuleFor(s => s.LastName, f => f.Name.LastName())
                    .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.FirstName, s.LastName))
                    .Generate(100);

                await db.Students.AddRangeAsync(students);

                // 2. Cursos fijos
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

                // Guardar estudiantes y cursos para que tengan Ids asignados
                await db.SaveChangesAsync();

                // 3. Crear semestres e inscripciones
                var semesterTerms = new[] { "Primavera", "Otoño" };
                var random = new Random();

                var semesterEnrollments = new List<SemesterEnrollment>();
                var enrolledCourses = new List<EnrolledCourse>();

                foreach (var student in students)
                {
                    // 1 o 2 semestres por estudiante
                    int semestersCount = random.Next(1, 3);

                    for (int i = 0; i < semestersCount; i++)
                    {
                        var semester = new SemesterEnrollment
                        {
                            StudentId = student.Id,
                            Year = 2023 + random.Next(0, 3), // 2023 a 2025
                            Term = semesterTerms[random.Next(semesterTerms.Length)],
                            MaxCreditHours = 21,
                            Courses = new List<EnrolledCourse>()
                        };

                        semesterEnrollments.Add(semester);

                        // 2 a 4 cursos por semestre
                        int coursesCount = random.Next(2, 5);
                        var selectedCourses = courses.OrderBy(_ => random.Next()).Take(coursesCount).ToList();

                        foreach (var course in selectedCourses)
                        {
                            var enrolled = new EnrolledCourse
                            {
                                CourseId = course.Id,
                                CreditHours = course.CreditHours,
                                SemesterEnrollment = semester
                            };
                            enrolledCourses.Add(enrolled);
                            semester.Courses.Add(enrolled);
                        }
                    }
                }

                await db.SemesterEnrollments.AddRangeAsync(semesterEnrollments);
                await db.EnrolledCourses.AddRangeAsync(enrolledCourses);

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
