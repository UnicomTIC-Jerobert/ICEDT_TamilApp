using System;

namespace ICEDT_TamilApp.Application.DTOs.Response
{
    public class LessonPdfResponseDto
    {
        public int LessonPdfId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }
}
