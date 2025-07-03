<<<<<<< HEAD
using InscripcionUniAPI.Core.DTOs;
using InscripcionUniAPI.Core.Entities;
=======
using InscripcionUniAPI.Core.Dtos;
>>>>>>> 8dfe60f3f6676b7b6824560c9e3969130bd59001
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
