
using Microsoft.AspNetCore.Mvc;
using ICEDT_TamilApp.Web.Middlewares;
using ICEDT_TamilApp.Application.Services.Interfaces;
using ICEDT_TamilApp.Application.Exceptions;
using ICEDT_TamilApp.Application.DTOs.Request;


namespace ICEDT_TamilApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _service;

        public ActivitiesController(IActivityService service) => _service = service;

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid Activity ID.");
            var activity = await _service.GetActivityAsync(id);
            return Ok(activity);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllActivitiesAsync());

        [HttpGet("by-lesson")]
        public async Task<IActionResult> GetActivitiesByLessonId(
            [FromQuery] int lessonId,
            [FromQuery] int? activityTypeId,
            [FromQuery] int? mainActivityTypeId)
        {
            if (lessonId <= 0)
                throw new BadRequestException("Invalid Lesson ID.");
            var activities = await _service.GetActivitiesByLessonIdAsync(lessonId);
            return Ok(activities);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActivityRequestDto dto)
        {
            var activity = await _service.CreateActivityAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = activity.ActivityId }, activity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActivityRequestDto dto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid Activity ID.");
            await _service.UpdateActivityAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid Activity ID.");
            await _service.DeleteActivityAsync(id);
            return NoContent();
        }


    }
}