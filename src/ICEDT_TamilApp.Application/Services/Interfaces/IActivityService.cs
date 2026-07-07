using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface IActivityService
    {
        Task<ActivityResponseDto> GetActivityAsync(int id, CancellationToken cancellationToken = default);
        Task<List<ActivityResponseDto>> GetAllActivitiesAsync(CancellationToken cancellationToken = default);
        Task<List<ActivityResponseDto>> GetActivitiesByLessonIdAsync(int lessonId, CancellationToken cancellationToken = default);
        Task<ActivityResponseDto> CreateActivityAsync(ActivityRequestDto dto, CancellationToken cancellationToken = default);
        Task<ActivityResponseDto> UpdateActivityAsync(int id, ActivityRequestDto dto, CancellationToken cancellationToken = default);
        Task DeleteActivityAsync(int id, CancellationToken cancellationToken = default);
    }
}
