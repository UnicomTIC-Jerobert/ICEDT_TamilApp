using System.Security.Claims;
using System.Threading.Tasks;
using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using ICEDT_TamilApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICEDT_TamilApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // All actions in this controller require a valid JWT
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        /// <summary>
        /// Gets the current lesson and all its activities for the logged-in user.
        /// This is the main endpoint the app will call after login.
        /// </summary>
        [HttpGet("current-lesson")]
        [ProducesResponseType(typeof(CurrentLessonResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCurrentLesson()
        {
            var userId = GetUserId();
            var currentLessonData = await _progressService.GetCurrentLessonForUserAsync(userId);

            if (currentLessonData == null)
            {
                return NotFound("Could not find a current lesson for the user.");
            }

            return Ok(currentLessonData);
        }

        /// <summary>
        /// Marks an activity as complete for the user and handles unlocking the next lesson.
        /// </summary>
        [HttpPost("complete-activity")]
        [ProducesResponseType(typeof(ActivityCompletionResponseDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CompleteActivity(
            [FromBody] ActivityCompletionRequestDto request
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserId();
            var response = await _progressService.CompleteActivityAsync(userId, request);

            return Ok(response);
        }

        /// <summary>
        /// Helper method to safely extract UserId from the JWT claims.
        /// </summary>
        private int GetUserId()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdValue) || !int.TryParse(userIdValue, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }
            return userId;
        }
    }
}
