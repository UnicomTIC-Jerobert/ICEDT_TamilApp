using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.Exceptions;
using ICEDT_TamilApp.Application.Services.Interfaces;
using ICEDT_TamilApp.Web.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace ICEDT_TamilApp.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _service;

        public ActivitiesController(IActivityService service) => _service = service;

        [HttpGet("activities/{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid Activity ID.");
            var activity = await _service.GetActivityAsync(id);
            return Ok(activity);
        }

        [HttpGet("activities")]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllActivitiesAsync());

        [HttpGet("lessons/{lessonId:int}/activities")]
        public async Task<IActionResult> GetActivitiesByLessonId(int lessonId)
        {
            if (lessonId <= 0)
                throw new BadRequestException("Invalid Lesson ID.");
            var activities = await _service.GetActivitiesByLessonIdAsync(lessonId);
            return Ok(activities);
        }

        [HttpPost("activities")]
        public async Task<IActionResult> Create([FromBody] ActivityRequestDto dto)
        {
            var activity = await _service.CreateActivityAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = activity.ActivityId }, activity);
        }

        [HttpPut("activities/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActivityRequestDto dto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid Activity ID.");
            await _service.UpdateActivityAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("activities/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid Activity ID.");
            await _service.DeleteActivityAsync(id);
            return NoContent();
        }
    }
}
