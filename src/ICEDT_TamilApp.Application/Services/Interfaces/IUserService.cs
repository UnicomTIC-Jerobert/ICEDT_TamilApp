namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task<UserDto> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<UserDto> CreateUserAsync(CreateUserRequestDto dto, CancellationToken cancellationToken = default);
        Task<UserDto> UpdateUserAsync(int id, UpdateUserRequestDto dto, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(int id, CancellationToken cancellationToken = default);
    }
}
