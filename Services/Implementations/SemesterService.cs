using InscripcionUniAPI.Services.Interfaces;
using InscripcionUniAPI.Data;
using InscripcionUniAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InscripcionUniAPI.Services.Implementations
{
    public class SemesterService : ISemesterService
    {
        private readonly UniversityDbContext _db;

        public SemesterService(UniversityDbContext db)
        {
            _db = db;
        }

        public async Task<List<Semester>> GetAllAsync()
        {
            return await _db.Semesters.Include(s => s.SemesterCourses).ToListAsync();
        }

        public async Task<Semester?> GetByIdAsync(int id)
        {
            return await _db.Semesters
                .Include(s => s.SemesterCourses)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Semester> CreateAsync(Semester semester)
        {
            _db.Semesters.Add(semester);
            await _db.SaveChangesAsync();
            return semester;
        }

        public async Task<Semester?> UpdateAsync(int id, Semester semester)
        {
            var existing = await _db.Semesters.FindAsync(id);
            if (existing == null) return null;

            existing.Name = semester.Name;
            existing.Year = semester.Year;
            // Actualiza otras propiedades si las hay

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var semester = await _db.Semesters.FindAsync(id);
            if (semester == null) return false;

<<<<<<< HEAD
            if (semester == null)
                throw new KeyNotFoundException($"Semester with id {semesterId} not found.");

            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                throw new KeyNotFoundException($"Course with id {courseId} not found.");

            // Crear la relación entre semestre y curso
            var enrolledCourse = new EnrolledCourse
            {
                SemesterEnrollmentId = semesterId,
                CourseId = courseId,
                CreditHours = course.CreditHours
            };

            _context.EnrolledCourses.Add(enrolledCourse);
            await _context.SaveChangesAsync();

            // Recargar el semestre con los cursos para devolver la respuesta actualizada
            return await GetByIdAsync(semesterId);
=======
            _db.Semesters.Remove(semester);
            await _db.SaveChangesAsync();
            return true;
>>>>>>> 8dfe60f3f6676b7b6824560c9e3969130bd59001
        }
    }
}
