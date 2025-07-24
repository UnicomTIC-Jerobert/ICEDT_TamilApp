using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ICEDT_TamilApp.Application.DTOs.Requst
{
    public class MediaUploadRequestDto
    {
        [Required]
        public IFormFile File { get; set; }

        public string Folder { get; set; } = "general";
    }
}
