using ICEDT_TamilApp.Domain.Entities;
using System.Threading.Tasks;

public interface IProgressRepository
{
    Task CreateInitialProgressAsync(int userId, int firstLessonId);
    Task<UserCurrentProgress?> GetCurrentProgressAsync(int userId);
    Task<int> GetTotalActivitiesForLessonAsync(int lessonId);
    Task<int> GetCompletedActivitiesCountAsync(int userId, int lessonId); // Renamed for clarity
    Task MarkActivityAsCompleteAsync(int userId, int activityId, int? score);
    Task UpdateCurrentLessonAsync(int userId, int newLessonId);
    Task<Lesson?> GetNextLessonAsync(int currentLessonId);
    Task<Lesson?> GetFirstLessonAsync(); // Added this method
    Task<Activity?> GetActivityByIdAsync(int activityId); // Added for convenience
}