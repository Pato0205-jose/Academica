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
            if (await db.EnrolledCourses.AnyAsync())
                return;

            List<EnrolledCourse> enrolledCourses = GetEnrolledCourses();

            await db.EnrolledCourses.AddRangeAsync(enrolledCourses);
            await db.SaveChangesAsync();
        }

        private static List<EnrolledCourse> GetEnrolledCourses()
        {
            return new List<EnrolledCourse>();
        }
    }
}
