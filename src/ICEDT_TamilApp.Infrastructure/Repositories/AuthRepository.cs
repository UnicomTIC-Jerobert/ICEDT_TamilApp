using ICEDT_TamilApp.Domain.Entities;
using ICEDT_TamilApp.Domain.Interfaces;
using ICEDT_TamilApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ICEDT_TamilApp.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            // --- THE FIX IS HERE ---
            // Convert both the database column and the input parameter to the same case.
            // EF Core can easily translate .ToLower() into the SQL LOWER() function.
            var normalizedUsername = username.ToLower();
            return await _context.Users.FirstOrDefaultAsync(u =>
                u.Username.ToLower() == normalizedUsername
            );
        }

        public async Task<bool> UserExistsAsync(string username, string email)
        {
            // --- AND APPLY THE FIX HERE AS WELL ---
            var normalizedUsername = username.ToLower();
            var normalizedEmail = email.ToLower();
            return await _context.Users.AnyAsync(u =>
                u.Username.ToLower() == normalizedUsername || u.Email.ToLower() == normalizedEmail
            );
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var normalizedEmail = email.ToLower();
            return await _context.Users.FirstOrDefaultAsync(u =>
                u.Email.ToLower() == normalizedEmail
            );
        }

        public async Task<User?> GetUserByPasswordResetOTPAsync(string email, string otp)
        {
            var normalizedEmail = email.ToLower();
            return await _context.Users.FirstOrDefaultAsync(u =>
                u.Email.ToLower() == normalizedEmail &&
                u.PasswordResetOTP == otp &&
                u.PasswordResetOTPExpiryTime > DateTime.UtcNow
            );
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
