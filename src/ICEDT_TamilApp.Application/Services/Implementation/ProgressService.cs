using ICEDT_TamilApp.Application.DTOs.Response;
using ICEDT_TamilApp.Domain.Interfaces;

public class ProgressService : IProgressService
{
    private readonly IProgressRepository _progressRepository;
    private readonly ILessonRepository _lessonRepository; // To fetch lesson data
    public ProgressService(IProgressRepository progressRepository, ILessonRepository lessonRepository)
    {
        _progressRepository = progressRepository;
        _lessonRepository = lessonRepository;
    }

    public async Task<LessonResponseDto> GetCurrentLessonForUserAsync(int userId)
    {
        var currentProgress = await _progressRepository.GetCurrentProgressAsync(userId);
        if (currentProgress == null)
        {
            // This case handles a brand new user. Find the very first lesson.
            var firstLesson = await _progressRepository.GetFirstLessonEver(); // You'd need to implement this
            if (firstLesson == null) throw new Exception("No lessons found in the system.");

            await _progressRepository.CreateInitialProgressAsync(userId, firstLesson.LessonId);
            return await _lessonRepository.GetLessonByIdAsync(firstLesson.LessonId); // Fetch and return the DTO
        }

        return await _lessonRepository.GetLessonByIdAsync(currentProgress.CurrentLessonId); // Fetch and return DTO
    }

    public async Task<bool> CompleteActivityAsync(int userId, int activityId, int? score)
    {
        // 1. Mark the activity as complete
        await _progressRepository.MarkActivityAsCompleteAsync(userId, activityId, score);

        // 2. Check if the lesson is now fully complete
        var activity = await _progressRepository.GetActivityByIdAsync(activityId); // Implement this
        int totalActivities = await _progressRepository.GetTotalActivitiesForLessonAsync(activity.LessonId);
        int completedActivities = await _progressRepository.GetCompletedActivitiesForLessonAsync(userId, activity.LessonId);

        if (completedActivities >= totalActivities)
        {
            // 3. If yes, unlock the next lesson
            var nextLesson = await _progressRepository.GetNextLessonAsync(activity.LessonId);
            if (nextLesson != null)
            {
                await _progressRepository.UpdateCurrentLessonAsync(userId, nextLesson.LessonId);
                return true; // Return true to indicate a new lesson was unlocked
            }
        }
        return false; // No new lesson unlocked
    }
}