using System.ComponentModel.DataAnnotations;

namespace ICEDT_TamilApp.Application.DTOs.Requst
{
    public class MediaUrlRequestDto
    {
        [Required]
        public string Key { get; set; }

        [Range(1, 1440)] // 1 minute to 24 hours
        public int ExpiryMinutes { get; set; } = 60;
    }
}