// TamilApp.Api/Controllers/ProgressController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TamilApp.Core.Dtos; // For ActivityCompletionDto
using TamilApp.Infrastructure.Services;

[ApiController]
[Route("api/[controller]")]
[Authorize] // IMPORTANT: All endpoints here require a logged-in user
public class ProgressController : ControllerBase
{
    private readonly IProgressService _progressService;
    public ProgressController(IProgressService progressService)
    {
        _progressService = progressService;
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("current-lesson")]
    public async Task<IActionResult> GetCurrentLesson()
    {
        var userId = GetUserId();
        var lesson = await _progressService.GetCurrentLessonForUserAsync(userId);
        if (lesson == null)
        {
            return NotFound("Could not determine current lesson for the user.");
        }
        // Here you would fetch all activities for this lesson and return them
        // var activities = await _activityService.GetActivitiesByLessonIdAsync(lesson.LessonId);
        // return Ok(new { lesson, activities });
        return Ok(lesson); 
    }

    [HttpPost("complete-activity")]
    public async Task<IActionResult> CompleteActivity([FromBody] ActivityCompletionRequestDto dto)
    {
        var userId = GetUserId();
        var newLessonUnlocked = await _progressService.CompleteActivityAsync(userId, dto.ActivityId, dto.Score);
        
        return Ok(new { NewLessonUnlocked = newLessonUnlocked });
    }
}