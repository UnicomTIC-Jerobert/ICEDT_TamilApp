using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface ILessonPdfService
    {
        Task<List<LessonPdfResponseDto>> GetPdfsForLessonAsync(int lessonId, CancellationToken cancellationToken = default);
        Task<LessonPdfResponseDto> UploadPdfAsync(int lessonId, LessonPdfUploadRequestDto request, CancellationToken cancellationToken = default);
        Task DeletePdfAsync(int lessonPdfId, CancellationToken cancellationToken = default);
    }
}
