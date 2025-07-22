using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface IActivityService
    {
        // Activity
        Task<ActivityResponseDto> GetActivityAsync(int id);
        Task<List<ActivityResponseDto>> GetAllActivitiesAsync();
        Task<List<ActivityResponseDto>> GetActivitiesByLessonIdAsync(
            int lessonId,
            int? activitytypeid,
            int? mainactivitytypeid
        );
        Task<ActivityResponseDto> AddActivityAsync(ActivityRequestDto dto);
        Task UpdateActivityAsync(int id, ActivityRequestDto dto);
        Task DeleteActivityAsync(int id);

        // ActivityType
        Task<ActivityTypeResponseDto> GetActivityTypeAsync(int id);
        Task<List<ActivityTypeResponseDto>> GetAllActivityTypesAsync();
        Task<ActivityTypeResponseDto> AddActivityTypeAsync(ActivityTypeRequestDto dto);
        Task UpdateActivityTypeAsync(int id, ActivityTypeRequestDto dto);
        Task DeleteActivityTypeAsync(int id);
    }
}
