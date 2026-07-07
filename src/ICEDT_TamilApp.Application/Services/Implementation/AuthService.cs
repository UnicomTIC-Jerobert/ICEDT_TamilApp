using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ICEDT_TamilApp.Application.Common;
using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using ICEDT_TamilApp.Application.Services.Interfaces;
using ICEDT_TamilApp.Domain.Entities;
using ICEDT_TamilApp.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ICEDT_TamilApp.Application.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUnitOfWork unitOfWork,
            IOptions<JwtSettings> jwtOptions,
            IEmailService emailService,
            IConfiguration configuration
        )
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtOptions.Value;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerDto)
        {
            if (await _unitOfWork.Auth.UserExistsAsync(registerDto.Username, registerDto.Email))
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Username or Email already exists.",
                };
            }

            // Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Role = "Student",
            };

            var token = CreateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);

            await _unitOfWork.Auth.RegisterUserAsync(user);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "User registered successfully.",
                Token = token,
                RefreshToken = refreshToken,
                Role = user.Role,
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto)
        {
            var user = await _unitOfWork.Auth.GetUserByUsernameAsync(loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid username or password.",
                };
            }

            // Create JWT Token
            var token = CreateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);

            await _unitOfWork.CompleteAsync();

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Login successful.",
                Token = token,
                RefreshToken = refreshToken,
                Role = user.Role,
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _unitOfWork.Auth.GetUserByRefreshTokenAsync(refreshToken); // New repository method needed

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid or expired refresh token.",
                };
            }

            var newAccessToken = CreateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();

            // Update the user's refresh token with a new one (token rotation)
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);

            await _unitOfWork.CompleteAsync();

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Token refreshed successfully.",
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Role = user.Role,
            };
        }

        private string CreateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };

            if (string.IsNullOrEmpty(_jwtSettings.Secret))
                throw new Exception("JWT Secret is not configured!");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.ExpiryDays),
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<AuthResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto dto)
        {
            var user = await _unitOfWork.Auth.GetUserByEmailAsync(dto.Email);

            if (user == null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = true,
                    Message = "If an account exists with this email, an OTP has been sent.",
                };
            }

            // Generate a 6-digit OTP
            string otp = GenerateOTP();
            user.PasswordResetOTP = otp;
            user.PasswordResetOTPExpiryTime = DateTime.UtcNow.AddMinutes(15); // OTP valid for 15 minutes

            await _unitOfWork.Auth.UpdateUserAsync(user);

            // Send email with OTP
            try
            {
                var emailBody =
                    $@"
                    <html>
                        <body style='font-family: Arial, sans-serif;'>
                            <h2>Password Reset OTP</h2>
                            <p>Your One-Time Password (OTP) for password reset is:</p>
                            <p style='font-size: 24px; font-weight: bold; letter-spacing: 5px; background-color: #f0f0f0; padding: 10px; text-align: center;'>{otp}</p>
                            <p>This OTP is valid for <strong>15 minutes</strong>.</p>
                            <p>If you didn't request this, please ignore this email.</p>
                        </body>
                    </html>";

                await _emailService.SendEmailAsync(user.Email, "Password Reset OTP", emailBody);
            }
            catch (Exception ex)
            {
                // Log the error but don't fail the request
                // In production, you might want to log this properly
                Console.WriteLine($"Error sending password reset OTP email: {ex.Message}");
            }

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "If an account exists with this email, an OTP has been sent.",
            };
        }

        public async Task<AuthResponseDto> VerifyOTPAsync(VerifyOTPRequestDto dto)
        {
            var user = await _unitOfWork.Auth.GetUserByPasswordResetOTPAsync(dto.Email, dto.OTP);

            if (user == null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid or expired OTP.",
                };
            }

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "OTP verified successfully. You can now reset your password.",
            };
        }

        private string GenerateOTP()
        {
            return RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        }

        public async Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            var user = await _unitOfWork.Auth.GetUserByPasswordResetOTPAsync(dto.Email, dto.OTP);

            if (user == null || user.PasswordResetOTPExpiryTime <= DateTime.UtcNow)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid or expired OTP.",
                };
            }

            // Hash the new password
            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            // Update user password and clear reset OTP
            user.PasswordHash = newPasswordHash;
            user.PasswordResetOTP = null;
            user.PasswordResetOTPExpiryTime = null;

            await _unitOfWork.Auth.UpdateUserAsync(user);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message =
                    "Password has been reset successfully. You can now login with your new password.",
            };
        }
    }
}
