using InscripcionUniAPI.Core.DTOs;
using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace InscripcionUniAPI.Controllers
{
    [ApiController]
    [Route("api/students/{studentId:int}/semesters")]
    [Authorize]
    public class SemestersController : ControllerBase
    {
        private readonly ISemesterService _service;

        public SemestersController(ISemesterService service) => _service = service;

        // ---------- POST: api/students/{studentId}/semesters ----------
        [HttpPost]
        public async Task<IActionResult> StartSemester(int studentId, [FromBody] StartSemesterDto dto)
        {
            var result = await _service.StartSemesterAsync(studentId, dto);
            return CreatedAtAction(nameof(GetSemester), new { studentId, semesterId = result.Id }, result);
        }

        // ---------- GET: api/students/{studentId}/semesters/{semesterId} ----------
        [HttpGet("{semesterId:int}")]
        public async Task<IActionResult> GetSemester(int studentId, int semesterId)
        {
            var semester = await _service.GetByIdAsync(semesterId);
            return semester is null
                ? NotFound()
                : Ok(semester);
        }

        // ---------- POST: api/students/{studentId}/semesters/{semesterId}/courses/{courseId} ----------
        [HttpPost("{semesterId:int}/courses/{courseId:int}")]
        public async Task<IActionResult> AddCourse(int studentId, int semesterId, int courseId)
        {
            try
            {
                var semester = await _service.AddCourseAsync(semesterId, courseId);
                return Ok(semester);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
