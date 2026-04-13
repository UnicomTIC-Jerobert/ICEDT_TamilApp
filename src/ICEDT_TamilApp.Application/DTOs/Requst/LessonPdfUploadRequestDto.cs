using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ICEDT_TamilApp.Application.DTOs.Request
{
    public class LessonPdfUploadRequestDto
    {
        [Required]
        [MaxLength(200)]
        public required string Title { get; set; }

        [Required]
        public required IFormFile File { get; set; }
    }
}
