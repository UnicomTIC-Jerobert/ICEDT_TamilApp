using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface IActivityTypeService
    {
        Task<ActivityTypeResponseDto> GetActivityTypeAsync(int id, CancellationToken cancellationToken = default);
        Task<List<ActivityTypeResponseDto>> GetAllActivityTypesAsync(CancellationToken cancellationToken = default);
        Task<ActivityTypeResponseDto> AddActivityTypeAsync(ActivityTypeRequestDto dto, CancellationToken cancellationToken = default);
        Task UpdateActivityTypeAsync(int id, ActivityTypeRequestDto dto, CancellationToken cancellationToken = default);
        Task DeleteActivityTypeAsync(int id, CancellationToken cancellationToken = default);
    }
}
