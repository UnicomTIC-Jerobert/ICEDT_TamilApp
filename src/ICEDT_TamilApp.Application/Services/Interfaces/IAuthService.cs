using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerDto, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordDto, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> VerifyOTPAsync(VerifyOTPRequestDto verifyOTPDto, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordDto, CancellationToken cancellationToken = default);
    }
}
