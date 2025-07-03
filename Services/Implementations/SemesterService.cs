using InscripcionUniAPI.Core.Dtos;
using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Data;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
                Courses = new List<EnrolledCourseDto>() // vacío al iniciar
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
                Courses = semester.Courses.Select(sc => new EnrolledCourseDto
                {
                    Id = sc.Id,
                    CourseId = sc.CourseId,
                    Code = sc.Course.Code,
                    Name = sc.Course.Name,
                    CreditHours = sc.Course.CreditHours
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

            // Verificar si el curso ya está inscrito
            bool courseExists = semester.Courses.Any(sc => sc.CourseId == courseId);
            if (courseExists)
                throw new InvalidOperationException("Course already enrolled in this semester.");

            var enrolledCourse = new EnrolledCourse
            {
                SemesterEnrollmentId = semesterId,
                CourseId = courseId,
                CreditHours = course.CreditHours
            };

            _context.EnrolledCourses.Add(enrolledCourse);
            await _context.SaveChangesAsync();

            // Recargar semestre actualizado
            semester = await _context.SemesterEnrollments
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
                Courses = semester.Courses.Select(sc => new EnrolledCourseDto
                {
                    Id = sc.Id,
                    CourseId = sc.CourseId,
                    Code = sc.Course.Code,
                    Name = sc.Course.Name,
                    CreditHours = sc.Course.CreditHours
                }).ToList()
            };
        }
    }
}
