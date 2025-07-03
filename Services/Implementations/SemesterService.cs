using InscripcionUniAPI.Core.Dtos;
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

        public async Task<SemesterEnrollmentResponseDto> StartSemesterAsync(int studentId, StartSemesterDto dto)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
                throw new KeyNotFoundException($"Student with id {studentId} not found.");

            var semester = new SemesterEnrollment
            {
                StudentId = studentId,
                Year = dto.Year,
                Term = dto.Term,
                MaxCreditHours = dto.MaxCreditHours
            };

            _context.SemesterEnrollments.Add(semester);
            await _context.SaveChangesAsync();

            return new SemesterEnrollmentResponseDto
            {
                Id = semester.Id,
                StudentId = semester.StudentId,
                Year = semester.Year,
                Term = semester.Term,
                MaxCreditHours = semester.MaxCreditHours,
                Courses = new List<CourseDto>()
            };
        }

        public async Task<SemesterEnrollmentResponseDto?> GetByIdAsync(int semesterId)
        {
            var semester = await _context.SemesterEnrollments
                .Include(se => se.Courses)
                    .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(se => se.Id == semesterId);

            if (semester == null) return null;

            return new SemesterEnrollmentResponseDto
            {
                Id = semester.Id,
                StudentId = semester.StudentId,
                Year = semester.Year,
                Term = semester.Term,
                MaxCreditHours = semester.MaxCreditHours,
                Courses = semester.Courses.Select(c => new CourseDto
                {
                    Id = c.Course.Id,
                    Code = c.Course.Code,
                    Name = c.Course.Name,
                    CreditHours = c.Course.CreditHours
                }).ToList()
            };
        }

        public async Task<SemesterEnrollmentResponseDto?> AddCourseAsync(int semesterId, int courseId)
        {
            var semester = await _context.SemesterEnrollments
                .Include(se => se.Courses)
                    .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(se => se.Id == semesterId);

            if (semester == null)
                throw new KeyNotFoundException($"Semester with id {semesterId} not found.");

            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                throw new KeyNotFoundException($"Course with id {courseId} not found.");

            var semesterCourse = new SemesterCourse
            {
                SemesterEnrollmentId = semesterId,
                CourseId = courseId,
                CreditHours = course.CreditHours
            };

            _context.SemesterCourses.Add(semesterCourse);
            await _context.SaveChangesAsync();

            semester.Courses.Add(semesterCourse); // Para reflejar el cambio en la respuesta

            return new SemesterEnrollmentResponseDto
            {
                Id = semester.Id,
                StudentId = semester.StudentId,
                Year = semester.Year,
                Term = semester.Term,
                MaxCreditHours = semester.MaxCreditHours,
                Courses = semester.Courses.Select(c => new CourseDto
                {
                    Id = c.Course.Id,
                    Code = c.Course.Code,
                    Name = c.Course.Name,
                    CreditHours = c.Course.CreditHours
                }).ToList()
            };
        }
    }
}
