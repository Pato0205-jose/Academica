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
        public DbSet<SemesterCourse> SemesterCourses { get; set; } = null!;
        public DbSet<EnrolledCourse> EnrolledCourses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración clave primaria
            modelBuilder.Entity<SemesterCourse>()
                .HasKey(sc => sc.Id);

            modelBuilder.Entity<EnrolledCourse>()
                .HasKey(ec => ec.Id);

            // Relaciones SemesterCourse <-> SemesterEnrollment
            modelBuilder.Entity<SemesterCourse>()
                .HasOne(sc => sc.SemesterEnrollment)
                .WithMany(se => se.Courses)
                .HasForeignKey(sc => sc.SemesterEnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relaciones SemesterCourse <-> Course
            modelBuilder.Entity<SemesterCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.SemesterCourses)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relaciones EnrolledCourse <-> SemesterEnrollment
            modelBuilder.Entity<EnrolledCourse>()
                .HasOne(ec => ec.SemesterEnrollment)
                .WithMany()
                .HasForeignKey(ec => ec.SemesterEnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relaciones EnrolledCourse <-> Course
            modelBuilder.Entity<EnrolledCourse>()
                .HasOne(ec => ec.Course)
                .WithMany()
                .HasForeignKey(ec => ec.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configura otras entidades y restricciones si necesitas
        }
    }
}
