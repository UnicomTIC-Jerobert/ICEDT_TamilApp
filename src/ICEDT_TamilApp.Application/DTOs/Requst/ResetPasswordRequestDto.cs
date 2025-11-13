using System.ComponentModel.DataAnnotations;

namespace ICEDT_TamilApp.Application.DTOs.Request
{
    public class ResetPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(6, MinimumLength = 6)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "OTP must contain only numbers")]
        public string OTP { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
