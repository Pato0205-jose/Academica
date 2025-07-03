using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InscripcionUniAPI.Services.Implementations
{
    public class SemesterService : ISemesterService
    {
        private readonly Data.UniversityDbContext _db;

        public SemesterService(Data.UniversityDbContext db) => _db = db;

        public async Task<SemesterEnrollment> StartSemesterAsync(int studentId, SemesterEnrollment semester)
        {
            semester.StudentId = studentId;
            _db.SemesterEnrollments.Add(semester);
            await _db.SaveChangesAsync();
            return semester;
        }

        public async Task<EnrolledCourse> AddCourseAsync(int semesterId, int courseId)
        {
            var semester = await _db.SemesterEnrollments
                                    .Include(s => s.Courses)
                                    .FirstOrDefaultAsync(s => s.Id == semesterId)
                        ?? throw new KeyNotFoundException("Semestre no encontrado");

            var course = await _db.Courses.FindAsync(courseId)
                         ?? throw new KeyNotFoundException("Curso no encontrado");

            semester.AddCourse(course);
            await _db.SaveChangesAsync();
            return semester.Courses.Last();
        }

        public async Task<SemesterEnrollment?> GetByIdAsync(int id) =>
            await _db.SemesterEnrollments
                     .Include(s => s.Courses)
                     .FirstOrDefaultAsync(s => s.Id == id);
    }
}
