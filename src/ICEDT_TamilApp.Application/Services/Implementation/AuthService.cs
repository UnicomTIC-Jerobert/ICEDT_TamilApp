using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ICEDT_TamilApp.Application.Common;
using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using ICEDT_TamilApp.Application.Services.Interfaces;
using ICEDT_TamilApp.Domain.Entities;
using ICEDT_TamilApp.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ICEDT_TamilApp.Application.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtSettings _jwtSettings; // Store the settings directly

        // Inject IOptions<JwtSettings>
        public AuthService(IAuthRepository authRepository, IOptions<JwtSettings> jwtOptions)
        {
            _authRepository = authRepository;
            _jwtSettings = jwtOptions.Value; // Get the actual settings object
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerDto)
        {
            if (await _authRepository.UserExistsAsync(registerDto.Username, registerDto.Email))
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
            };

            await _authRepository.RegisterUserAsync(user);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "User registered successfully.",
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto)
        {
            var user = await _authRepository.GetUserByUsernameAsync(loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid username or password.",
                };
            }

            // Create JWT Token
            var token = CreateToken(user);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Login successful.",
                Token = token,
            };
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            // Use the strongly-typed settings object now!
            if (string.IsNullOrEmpty(_jwtSettings.Secret))
                throw new Exception("JWT Secret is not configured!");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(_jwtSettings.ExpiryDays),
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
