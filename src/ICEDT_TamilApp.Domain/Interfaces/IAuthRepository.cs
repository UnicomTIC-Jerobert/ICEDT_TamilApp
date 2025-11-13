using ICEDT_TamilApp.Domain.Entities;

namespace ICEDT_TamilApp.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> UserExistsAsync(string username, string email);
        Task<User> RegisterUserAsync(User user);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByPasswordResetOTPAsync(string email, string otp);
        Task UpdateUserAsync(User user);
    }
}
