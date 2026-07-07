using Microsoft.AspNetCore.JsonPatch;
using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using Microsoft.AspNetCore.Http;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface ILevelService
    {
        Task<LevelResponseDto> GetLevelAsync(int id, CancellationToken cancellationToken = default);
        Task<List<LevelResponseDto>> GetAllLevelsAsync(CancellationToken cancellationToken = default);
        Task<LevelResponseDto> CreateLevelAsync(LevelRequestDto dto, CancellationToken cancellationToken = default);
        Task UpdateLevelAsync(int id, LevelRequestDto dto, CancellationToken cancellationToken = default);
        Task DeleteLevelAsync(int id, CancellationToken cancellationToken = default);

        Task<LevelResponseDto> UpdateLevelCoverImageAsync(int levelId, IFormFile file, CancellationToken cancellationToken = default);

        Task<LevelResponseDto?> PartialUpdateLevelAsync(int id, JsonPatchDocument<LevelUpdateRequestDto> patchDoc, CancellationToken cancellationToken = default);

        Task<LevelResponseDto> UnlockLevelByBarcodeAsync(int userId, string barcode, CancellationToken cancellationToken = default);
        Task<List<LevelResponseDto>> GetUnlockedLevelsForUserAsync(int userId, CancellationToken cancellationToken = default);
    }
}
