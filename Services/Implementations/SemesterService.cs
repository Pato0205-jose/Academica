using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Data;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InscripcionUniAPI.Services.Implementations
{
    public class SemesterService : ISemesterService
    {
        private readonly UniversityDbContext _context;

        public SemesterService(UniversityDbContext context)
        {
            _context = context;
        }

        public async Task<SemesterEnrollment> StartSemesterAsync(int studentId, SemesterEnrollment semester)
        {
            // Validar que el estudiante existe
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
                throw new KeyNotFoundException($"Student with id {studentId} not found.");

            // Asociar el studentId al semestre
            semester.StudentId = studentId;

            // Guardar el nuevo semestre
            _context.SemesterEnrollments.Add(semester);
            await _context.SaveChangesAsync();

            return semester;
        }

        public async Task<SemesterEnrollment?> GetByIdAsync(int semesterId)
        {
            return await _context.SemesterEnrollments
                .Include(se => se.Courses)
                    .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(se => se.Id == semesterId);
        }

        public async Task<SemesterEnrollment?> AddCourseAsync(int semesterId, int courseId)
        {
            var semester = await _context.SemesterEnrollments
                .Include(se => se.Courses)
                .FirstOrDefaultAsync(se => se.Id == semesterId);

            if (semester == null)
                throw new KeyNotFoundException($"Semester with id {semesterId} not found.");

            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                throw new KeyNotFoundException($"Course with id {courseId} not found.");

            // Aquí asumes que tienes una entidad puente entre semestre y curso, como SemesterCourse
            var semesterCourse = new SemesterCourse
            {
                SemesterEnrollmentId = semesterId,
                CourseId = courseId,
                CreditHours = course.CreditHours
            };

            _context.SemesterCourses.Add(semesterCourse);
            await _context.SaveChangesAsync();

            return semester;
        }
    }
}
