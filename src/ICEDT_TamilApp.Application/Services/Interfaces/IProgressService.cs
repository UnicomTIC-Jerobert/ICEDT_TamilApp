using ICEDT_TamilApp.Application.DTOs.Response;

public interface IProgressService
{
    Task<LessonResponseDto> GetCurrentLessonForUserAsync(int userId);
    Task<bool> CompleteActivityAsync(int userId, int activityId, int? score);
}