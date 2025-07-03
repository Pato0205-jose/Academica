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

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<SemesterEnrollment> SemesterEnrollments { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<EnrolledCourse> EnrolledCourses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración clave primaria
            modelBuilder.Entity<EnrolledCourse>()
                .HasKey(ec => ec.Id);

            // Relaciones EnrolledCourse <-> SemesterEnrollment
            modelBuilder.Entity<EnrolledCourse>()
                .HasOne(ec => ec.SemesterEnrollment)
                .WithMany(se => se.Courses)
                .HasForeignKey(ec => ec.SemesterEnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relaciones EnrolledCourse <-> Course
            modelBuilder.Entity<EnrolledCourse>()
                .HasOne(ec => ec.Course)
                .WithMany(c => c.EnrolledCourses)
                .HasForeignKey(ec => ec.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índice único para evitar duplicados
            modelBuilder.Entity<EnrolledCourse>()
                .HasIndex(e => new { e.SemesterEnrollmentId, e.CourseId })
                .IsUnique();
        }
    }
}
