using System.ComponentModel.DataAnnotations;

namespace ICEDT_TamilApp.Application.DTOs.Request
{
    public class LevelRequestDto
    {
        [Required(ErrorMessage = "Level name is required.")]
        [StringLength(100, ErrorMessage = "Level name cannot exceed 100 characters.")]
        public required string LevelName { get; set; }

        [Required(ErrorMessage = "Sequence order is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Sequence order must be a positive number.")]
        public int SequenceOrder { get; set; }
    }
}
