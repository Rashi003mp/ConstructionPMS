using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ConstructionPM.Application.DTOs;
using ConstructionPM.Application.Interfaces.Services;

namespace ConstrictionPM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {

        private readonly IProjectService _service;

        public ProjectsController(IProjectService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateProjectDto request)
        {
            await _service.CreateAsync(request);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var projects = await _service.GetByIdAsync(id);

            return projects == null ? NotFound() : Ok(projects);

        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var project=await _service.GetAllAsync();
            return Ok(project);

        }

    }
}
