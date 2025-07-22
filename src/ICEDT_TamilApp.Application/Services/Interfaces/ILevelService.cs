using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface ILevelService
    {
        Task<LevelResponseDto> GetLevelAsync(int id);
        Task<List<LevelResponseDto>> GetAllLevelsAsync();
        Task<LevelResponseDto> AddLevelAsync(LevelRequestDto dto);
        Task UpdateLevelAsync(int id, LevelRequestDto dto);
        Task DeleteLevelAsync(int id);
        Task<LessonResponseDto> AddLessonToLevelAsync(int levelId, LessonRequestDto dto);
        Task RemoveLessonFromLevelAsync(int levelId, int lessonId);
        Task<LevelWithLessonsResponseDto> GetLevelWithLessonsAsync(int levelId);
    }
}
