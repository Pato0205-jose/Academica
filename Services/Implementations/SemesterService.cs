using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InscripcionUniAPI.Services.Implementations
{
    public class SemesterService : ISemesterService
    {
        private readonly Data.UniversityDbContext _db;

        public SemesterService(Data.UniversityDbContext db) => _db = db;

        // Crea un semestre para el estudiante indicado
        public async Task<SemesterEnrollment> StartSemesterAsync(int studentId, SemesterEnrollment semester)
        {
            semester.StudentId = studentId;
            _db.SemesterEnrollments.Add(semester);
            await _db.SaveChangesAsync();
            return semester;
        }

        // Agrega un curso existente a un semestre existente
        public async Task<EnrolledCourse> AddCourseAsync(int semesterId, int courseId)
        {
            // Obtener semestre con sus cursos
            var semester = await _db.SemesterEnrollments
                                    .Include(s => s.Courses)
                                    .FirstOrDefaultAsync(s => s.Id == semesterId)
                            ?? throw new KeyNotFoundException("Semestre no encontrado");

            // Verificar que el curso exista
            var course = await _db.Courses.FindAsync(courseId)
                         ?? throw new KeyNotFoundException("Curso no encontrado");

            // Evitar duplicar el mismo curso en el semestre
            if (semester.Courses.Any(c => c.CourseId == courseId))
                throw new InvalidOperationException("El curso ya está inscrito en este semestre.");

            // Crear la inscripción
            var enrolledCourse = new EnrolledCourse
            {
                SemesterEnrollmentId = semester.Id,
                CourseId = course.Id,
                CreditHours = course.CreditHours
            };

            semester.Courses.Add(enrolledCourse);

            await _db.SaveChangesAsync();
            return enrolledCourse;
        }

        // Obtener semestre por Id con sus cursos
        public async Task<SemesterEnrollment?> GetByIdAsync(int id) =>
            await _db.SemesterEnrollments
                     .Include(s => s.Courses)
                     .FirstOrDefaultAsync(s => s.Id == id);
    }
}
