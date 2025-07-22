using ICEDT_TamilApp.Domain.Entities;

namespace ICEDT_TamilApp.Domain.Interfaces
{
    public interface IActivityRepository
    {
        Task<Activity> GetByIdAsync(int id);
        Task<List<Activity>> GetAllAsync();
        Task<List<Activity>> GetByLessonIdAsync(int lessonId, int? activitytypeid, int? mainactivitytypeid);
        Task AddAsync(Activity activity);
        Task UpdateAsync(Activity activity);
        Task DeleteAsync(int id);
        Task<bool> SequenceOrderExistsAsync(int sequenceOrder);
        Task<bool> LessonExistsAsync(int lessonId);
    }
}