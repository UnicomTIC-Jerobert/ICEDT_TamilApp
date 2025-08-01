using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ICEDT_TamilApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LevelsController : ControllerBase
    {
        private readonly ILevelService _service;

        public LevelsController(ILevelService service) => _service = service;

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Level ID." });
            var level = await _service.GetLevelAsync(id);
            return Ok(level);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllLevelsAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LevelRequestDto dto)
        {
            var level = await _service.CreateLevelAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = level.LevelId }, level);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] LevelRequestDto dto)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Level ID." });
            await _service.UpdateLevelAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Level ID." });
            await _service.DeleteLevelAsync(id);
            return NoContent();
        }
    }
}
