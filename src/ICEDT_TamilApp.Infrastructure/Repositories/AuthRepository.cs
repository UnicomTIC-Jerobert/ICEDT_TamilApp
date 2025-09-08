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
            return await _context.Users.FirstOrDefaultAsync(u => 
                string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)
            );
        }

        public async Task<bool> UserExistsAsync(string username, string email)
        {
            // FIX: Apply the same case-insensitive comparison here.
            return await _context.Users.AnyAsync(u =>
                string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase) || 
                string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase)
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
    }
}
