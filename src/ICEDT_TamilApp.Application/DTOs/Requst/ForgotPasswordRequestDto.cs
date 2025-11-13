using System.ComponentModel.DataAnnotations;

namespace ICEDT_TamilApp.Application.DTOs.Request
{
    public class ForgotPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
