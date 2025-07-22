
using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ICEDT_TamilApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _service;

        public LessonController(ILessonService service) => _service = service;

        [HttpPost("level/{levelId:int}/lessons")]
        public async Task<IActionResult> AddLesson(int levelId, [FromBody] LessonRequestDto dto)
        {
            if (levelId <= 0)
                return BadRequest(new { message = "Invalid Level ID." });
            var lesson = await _service.CreateLessonToLevelAsync(levelId, dto);
            return CreatedAtAction(nameof(GetLevelWithLessons), new { levelId }, lesson);
        }

        [HttpDelete("/level/{levelId:int}/lessons/{lessonId:int}")]
        public async Task<IActionResult> RemoveLesson(int levelId, int lessonId)
        {
            if (levelId <= 0 || lessonId <= 0)
                return BadRequest(new { message = "Invalid  ID." });
            await _service.RemoveLessonFromLevelAsync(levelId, lessonId);
            return NoContent();
        }

        [HttpGet("/lessons/{levelId:int}")]
        public async Task<IActionResult> GetLevelWithLessons(int levelId)
        {
            if (levelId <= 0)
                return BadRequest(new { message = "Invalid Level ID." });
            var result = await _service.GetLevelWithLessonsAsync(levelId);
            return Ok(result);
        }

        [HttpPut("lessons/{id}")]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] LessonRequestDto updateLessonDto) // You'll need to create UpdateLessonDto
        {
            var success = await _service.UpdateLessonAsync(id, updateLessonDto);
            if (!success)
            {
                return NotFound($"Lesson with ID {id} not found.");
            }
            return NoContent(); // Standard 204 response for a successful update
        }
        [HttpDelete("lessons/{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var success = await _service.DeleteLessonAsync(id);
            if (!success)
            {
                return NotFound($"Lesson with ID {id} not found.");
            }
            return NoContent(); // Standard 204 response for a successful delete
        }
    }
}