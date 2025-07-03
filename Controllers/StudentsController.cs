using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InscripcionUniAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentsController(IStudentService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            Response.Headers["X-Total-Count"] = result.TotalCount.ToString();
            return Ok(result.Items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var student = await _service.GetByIdAsync(id);
            return student is null ? NotFound() : Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Student student)
        {
            var created = await _service.CreateAsync(student);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Student student)
        {
            var updated = await _service.UpdateAsync(id, student);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
