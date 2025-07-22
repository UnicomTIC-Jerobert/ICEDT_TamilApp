using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using System.Threading.Tasks;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface IProgressService
    {
        Task<CurrentLessonResponseDto?> GetCurrentLessonForUserAsync(int userId);
        Task<ActivityCompletionResponseDto> CompleteActivityAsync(int userId, ActivityCompletionRequestDto request);
    }
}