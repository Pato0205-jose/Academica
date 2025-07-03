using InscripcionUniAPI.Core.DTOs;
using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InscripcionUniAPI.Controllers
{
    [ApiController]
    [Route("api/students/{studentId:int}/semesters")]
    [Authorize]
    public class SemestersController : ControllerBase
    {
        private readonly ISemesterService _service;

        public SemestersController(ISemesterService service) => _service = service;

        // ---------- Helpers ----------
        private static SemesterEnrollmentResponseDto MapToDto(SemesterEnrollment semester) =>
            new()
            {
                Id              = semester.Id,
                StudentId       = semester.StudentId,
                Year            = semester.Year,
                Term            = semester.Term,
                MaxCreditHours  = semester.MaxCreditHours,
                Courses = semester.Courses.Select(sc => new CourseDto
                {
                    Id          = sc.Course.Id,
                    Code        = sc.Course.Code,
                    Name        = sc.Course.Name,
                    CreditHours = sc.Course.CreditHours
                }).ToList()
            };

        // ---------- POST: api/students/{studentId}/semesters ----------
        [HttpPost]
        public async Task<IActionResult> StartSemester(int studentId, [FromBody] StartSemesterDto dto)
        {
            var semester = new SemesterEnrollment
            {
                Year           = dto.Year,
                Term           = dto.Term,
                MaxCreditHours = dto.MaxCreditHours
            };

            var created = await _service.StartSemesterAsync(studentId, semester);
            var response = MapToDto(created);

            return CreatedAtAction(nameof(GetSemester),
                                   new { studentId, semesterId = response.Id },
                                   response);
        }

        // ---------- GET: api/students/{studentId}/semesters/{semesterId} ----------
        [HttpGet("{semesterId:int}")]
        public async Task<IActionResult> GetSemester(int studentId, int semesterId)
        {
            var semester = await _service.GetByIdAsync(semesterId);
            return semester is null
                ? NotFound()
                : Ok(MapToDto(semester));
        }

        // ---------- POST: api/students/{studentId}/semesters/{semesterId}/courses/{courseId} ----------
        [HttpPost("{semesterId:int}/courses/{courseId:int}")]
        public async Task<IActionResult> AddCourse(int studentId, int semesterId, int courseId)
        {
            var semester = await _service.AddCourseAsync(semesterId, courseId);
            return semester is null
                ? NotFound()
                : Ok(MapToDto(semester));
        }
    }
}
