using InscripcionUniAPI.Core.Dtos;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/students/{studentId:int}/semesters")]
[Authorize]
public class SemestersController : ControllerBase
{
    private readonly ISemesterService _service;

    public SemestersController(ISemesterService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> StartSemester(int studentId, [FromBody] StartSemesterDto dto)
    {
        var result = await _service.StartSemesterAsync(studentId, dto);
        return CreatedAtAction(nameof(GetSemester), new { studentId, semesterId = result.Id }, result);
    }

    [HttpGet("{semesterId:int}")]
    public async Task<IActionResult> GetSemester(int studentId, int semesterId)
    {
        var semester = await _service.GetByIdAsync(semesterId);
        return semester == null ? NotFound() : Ok(semester);
    }
}
