using System.Threading.Tasks;
using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface IProgressService
    {
        Task<CurrentLessonResponseDto?> GetCurrentLessonForUserAsync(int userId, CancellationToken cancellationToken = default);
        Task<ActivityCompletionResponseDto> CompleteActivityAsync(int userId, ActivityCompletionRequestDto request, CancellationToken cancellationToken = default);
        Task<ProgressSummaryDto> GetUserProgressSummaryAsync(int userId, CancellationToken cancellationToken = default);
        Task<List<DetailedProgressDto>> GetDetailedProgressForUserAsync(int userId, CancellationToken cancellationToken = default);
    }
}
