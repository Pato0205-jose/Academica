using InscripcionUniAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InscripcionUniAPI.Data
{
    public class UniversityDbContext : DbContext
    {
        public UniversityDbContext(DbContextOptions<UniversityDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<SemesterEnrollment> SemesterEnrollments => Set<SemesterEnrollment>();
        public DbSet<EnrolledCourse> EnrolledCourses => Set<EnrolledCourse>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnrolledCourse>()
                .HasIndex(e => new { e.SemesterEnrollmentId, e.CourseId })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
