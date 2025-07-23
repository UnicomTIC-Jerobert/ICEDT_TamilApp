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

        public virtual UserCurrentProgress? UserCurrentProgress { get; set; }
    }
}
