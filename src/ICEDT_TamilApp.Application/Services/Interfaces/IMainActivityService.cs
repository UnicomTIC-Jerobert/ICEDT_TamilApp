using System.Collections.Generic;
using System.Threading.Tasks;
using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface IMainActivityService
    {
        Task<MainActivityResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<MainActivityResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<MainActivityResponseDto> CreateAsync(MainActivityRequestDto requestDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(int id, MainActivityRequestDto requestDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
