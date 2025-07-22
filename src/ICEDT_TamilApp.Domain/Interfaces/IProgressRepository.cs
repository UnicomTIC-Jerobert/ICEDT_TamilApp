using ICEDT_TamilApp.Domain.Entities;

public interface IProgressRepository
{
    Task CreateInitialProgressAsync(int userId, int firstLessonId);
    Task<UserCurrentProgress?> GetCurrentProgressAsync(int userId);
    Task<int> GetTotalActivitiesForLessonAsync(int lessonId);
    Task<int> GetCompletedActivitiesForLessonAsync(int userId, int lessonId);
    Task MarkActivityAsCompleteAsync(int userId, int activityId, int? score);
    Task UpdateCurrentLessonAsync(int userId, int newLessonId);
    Task<Lesson?> GetNextLessonAsync(int currentLessonId);
}