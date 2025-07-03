using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InscripcionUniAPI.Controllers
{
    [ApiController]
    [Route("api/students/{studentId:int}/semesters")]
    public class SemestersController : ControllerBase
    {
        private readonly ISemesterService _service;

        public SemestersController(ISemesterService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> StartSemester(int studentId, SemesterEnrollment semester)
        {
            var result = await _service.StartSemesterAsync(studentId, semester);
            return CreatedAtAction(nameof(GetSemester), new { studentId, semesterId = result.Id }, result);
        }

        [HttpGet("{semesterId:int}")]
        public async Task<IActionResult> GetSemester(int semesterId)
        {
            var semester = await _service.GetByIdAsync(semesterId);
            return semester is null ? NotFound() : Ok(semester);
        }

        [HttpPost("{semesterId:int}/courses/{courseId:int}")]
        public async Task<IActionResult> AddCourse(int semesterId, int courseId)
        {
            var result = await _service.AddCourseAsync(semesterId, courseId);
            return Ok(result);
        }
    }
}
