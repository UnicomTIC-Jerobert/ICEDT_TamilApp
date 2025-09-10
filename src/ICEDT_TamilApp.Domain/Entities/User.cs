// TamilApp.Core/Models/User.cs
namespace ICEDT_TamilApp.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public required string Role { get; set; }

        public virtual UserCurrentProgress? UserCurrentProgress { get; set; }
        public virtual ICollection<UserProgress>? UserProgresses { get; set; }
        public ICollection<UserLevelAccess> LevelAccesses { get; set; } = new List<UserLevelAccess>();

    }
}
