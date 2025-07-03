using InscripcionUniAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InscripcionUniAPI.Data
{
    public class UniversityDbContext : DbContext
    {
        public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options)
        {
        }

        public DbSet<Semester> Semesters { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<SemesterCourse> SemesterCourses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de relaciones

            modelBuilder.Entity<SemesterCourse>()
                .HasKey(sc => sc.Id);

            modelBuilder.Entity<SemesterCourse>()
                .HasOne(sc => sc.Semester)
                .WithMany(s => s.SemesterCourses)
                .HasForeignKey(sc => sc.SemesterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SemesterCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.SemesterCourses)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Puedes agregar configuraciones adicionales si lo necesitas
        }
    }
}
