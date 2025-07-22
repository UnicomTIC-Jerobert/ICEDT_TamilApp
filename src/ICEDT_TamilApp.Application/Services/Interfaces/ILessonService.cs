using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface ILessonService
    {
        Task<LessonResponseDto> CreateLessonToLevelAsync(int levelId, LessonRequestDto dto);
        Task RemoveLessonFromLevelAsync(int levelId, int lessonId);
        Task<LevelWithLessonsResponseDto> GetLevelWithLessonsAsync(int levelId);

        
        Task<LessonResponseDto?> GetLessonByIdAsync(int lessonId);
        Task<bool> UpdateLessonAsync(int lessonId, LessonRequestDto updateDto); // Create UpdateLessonDto
        Task<bool> DeleteLessonAsync(int lessonId);
    }
}