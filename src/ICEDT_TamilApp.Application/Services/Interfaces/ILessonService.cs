using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using Microsoft.AspNetCore.Http;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface ILessonService
    {
        Task<LessonResponseDto> CreateLessonToLevelAsync(int levelId, LessonRequestDto dto);
        Task RemoveLessonFromLevelAsync(int levelId, int lessonId);
        Task<List<LessonResponseDto>> GetLessonsByLevelIdAsync(int levelId);

        Task<LessonResponseDto?> GetLessonByIdAsync(int lessonId);
        Task<bool> UpdateLessonAsync(int lessonId, LessonRequestDto updateDto);
        Task<bool> DeleteLessonAsync(int lessonId);

        Task<LessonResponseDto> UpdateLessonImageAsync(int lessonId, IFormFile file);
    }
}
