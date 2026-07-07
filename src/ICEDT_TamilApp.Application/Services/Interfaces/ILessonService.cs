using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using Microsoft.AspNetCore.Http;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface ILessonService
    {
        Task<LessonResponseDto> CreateLessonToLevelAsync(int levelId, LessonRequestDto dto, CancellationToken cancellationToken = default);
        Task RemoveLessonFromLevelAsync(int levelId, int lessonId, CancellationToken cancellationToken = default);
        Task<List<LessonResponseDto>> GetLessonsByLevelIdAsync(int levelId, CancellationToken cancellationToken = default);

        Task<LessonResponseDto?> GetLessonByIdAsync(int lessonId, CancellationToken cancellationToken = default);
        Task<LessonResponseDto> UpdateLessonAsync(int lessonId, LessonRequestDto updateDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteLessonAsync(int lessonId, CancellationToken cancellationToken = default);

        Task<LessonResponseDto> UpdateLessonImageAsync(int lessonId, IFormFile file, CancellationToken cancellationToken = default);

        Task<List<MainActivityResponseDto>> GetMainActivitySummaryAsync(int lessonId, CancellationToken cancellationToken = default);
    }
}
