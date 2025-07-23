using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.Exceptions;
using ICEDT_TamilApp.Application.Services.Interfaces;
using ICEDT_TamilApp.Web.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace ICEDT_TamilApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityTypeController : ControllerBase
    {
        private readonly IActivityTypeService _service;

        public ActivityTypeController(IActivityTypeService service) => _service = service;

        // ActivityType CRUD

        [HttpGet("types/{id:int}")]
        public async Task<IActionResult> GetType(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ActivityType ID.");
            var type = await _service.GetActivityTypeAsync(id);
            return Ok(type);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetAllTypes()
        {
            var types = await _service.GetAllActivityTypesAsync();
            return Ok(types);
        }

        [HttpPost("types")]
        public async Task<IActionResult> CreateType([FromBody] ActivityTypeRequestDto dto)
        {
            var type = await _service.AddActivityTypeAsync(dto);
            return CreatedAtAction(nameof(GetType), new { id = type.ActivityTypeId }, type);
        }

        [HttpPut("types/{id:int}")]
        public async Task<IActionResult> UpdateType(int id, [FromBody] ActivityTypeRequestDto dto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ActivityType ID.");
            await _service.UpdateActivityTypeAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("types/{id:int}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ActivityType ID.");
            await _service.DeleteActivityTypeAsync(id);
            return NoContent();
        }
    }
}
