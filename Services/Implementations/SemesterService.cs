using InscripcionUniAPI.Services.Interfaces;
using InscripcionUniAPI.Data;
using InscripcionUniAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using InscripcionUniAPI.Core.DTOs;

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
            // Validar que el estudiante existe
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

            // Guardar el nuevo semestre
            _context.SemesterEnrollments.Add(semester);
            await _context.SaveChangesAsync();

            return MapToDto(semester);
        }

        public async Task<SemesterEnrollmentResponseDto?> GetByIdAsync(int semesterId)
        {
            var semester = await _context.SemesterEnrollments
                .Include(se => se.Courses)
                    .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(se => se.Id == semesterId);

            return semester != null ? MapToDto(semester) : null;
        }

        public async Task<SemesterEnrollmentResponseDto?> AddCourseAsync(int semesterId, int courseId)
        {
            var semester = await _context.SemesterEnrollments
                .Include(se => se.Courses)
                .FirstOrDefaultAsync(se => se.Id == semesterId);

            if (semester == null)
                throw new KeyNotFoundException($"Semester with id {semesterId} not found.");

            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                throw new KeyNotFoundException($"Course with id {courseId} not found.");

            // Validación de dominio: verificar que no se exceda el límite de créditos
            if (!semester.CanAddCourse(course.CreditHours))
            {
                var currentCredits = semester.GetCurrentCreditHours();
                var remainingCredits = semester.GetRemainingCreditHours();
                throw new InvalidOperationException(
                    $"No se puede agregar el curso '{course.Name}' ({course.CreditHours} créditos). " +
                    $"Créditos actuales: {currentCredits}, Límite: {semester.MaxCreditHours}, " +
                    $"Créditos disponibles: {remainingCredits}");
            }

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
        }

        private static SemesterEnrollmentResponseDto MapToDto(SemesterEnrollment semester)
        {
            return new SemesterEnrollmentResponseDto
            {
                Id = semester.Id,
                StudentId = semester.StudentId,
                Year = semester.Year,
                Term = semester.Term,
                MaxCreditHours = semester.MaxCreditHours,
                Courses = semester.Courses.Select(sc => new CourseDto
                {
                    Id = sc.Course.Id,
                    Code = sc.Course.Code,
                    Name = sc.Course.Name,
                    CreditHours = sc.Course.CreditHours
                }).ToList()
            };
        }
    }
}
