using System.Threading.Tasks;
using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.Exceptions;
using ICEDT_TamilApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ICEDT_TamilApp.Web.Controllers
{
    [ApiController]
    [Route("api/activitytypes")]
    public class ActivityTypeController : ControllerBase
    {
        private readonly IActivityTypeService _service;

        public ActivityTypeController(IActivityTypeService service) => _service = service;

        // GET: /api/activitytypes/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ActivityType ID.");
            var type = await _service.GetActivityTypeAsync(id);
            return Ok(type);
        }

        // GET: /api/activitytypes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var types = await _service.GetAllActivityTypesAsync();
            return Ok(types);
        }

        // POST: /api/activitytypes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActivityTypeRequestDto dto)
        {
            var type = await _service.AddActivityTypeAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = type.ActivityTypeId }, type);
        }

        // PUT: /api/activitytypes/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActivityTypeRequestDto dto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ActivityType ID.");
            await _service.UpdateActivityTypeAsync(id, dto);
            return NoContent();
        }

        // DELETE: /api/activitytypes/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ActivityType ID.");
            await _service.DeleteActivityTypeAsync(id);
            return NoContent();
        }
    }
}
