using InscripcionUniAPI.Core.Dtos;
using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Data;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
}
