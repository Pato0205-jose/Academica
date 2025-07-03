using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace InscripcionUniAPI.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(UniversityDbContext db)
        {
            if (await db.SemesterCourses.AnyAsync())
                return;

            List<EnrolledCourse> enrolledCourses = GetEnrolledCourses();

            var semesterCourses = enrolledCourses
                .Select(ec => new SemesterCourse
                {
                    Id = ec.Id,
                    CourseId = ec.CourseId,
                    SemesterEnrollmentId = ec.SemesterEnrollmentId,
                    CreditHours = ec.CreditHours,
                })
                .ToList();

            await db.SemesterCourses.AddRangeAsync(semesterCourses);
            await db.SaveChangesAsync();
        }

        private static List<EnrolledCourse> GetEnrolledCourses()
        {
            return new List<EnrolledCourse>();
        }
    }
}
