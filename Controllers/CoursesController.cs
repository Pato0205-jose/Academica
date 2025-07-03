using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InscripcionUniAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _service;

        public CoursesController(ICourseService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 10)
        {
            var result = await _service.GetPagedAsync(page, size);
            Response.Headers["X-Total-Count"] = result.TotalCount.ToString();
            return Ok(result.Items);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Course course)
        {
            var created = await _service.CreateAsync(course);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
    }
}
