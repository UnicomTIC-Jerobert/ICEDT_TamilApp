using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using ICEDT_TamilApp.Application.Exceptions;
using ICEDT_TamilApp.Application.Services.Interfaces;
using ICEDT_TamilApp.Domain.Entities;
using ICEDT_TamilApp.Domain.Interfaces;

namespace ICEDT_TamilApp.Application.Services.Implementation
{
    public class LessonPdfService : ILessonPdfService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileUploader _fileUploader;

        public LessonPdfService(IUnitOfWork unitOfWork, IFileUploader fileUploader)
        {
            _unitOfWork = unitOfWork;
            _fileUploader = fileUploader;
        }

        public async Task<List<LessonPdfResponseDto>> GetPdfsForLessonAsync(int lessonId)
        {
            var pdfs = await _unitOfWork.LessonPdfs.GetAllByLessonIdAsync(lessonId);
            return pdfs.Select(MapToDto).ToList();
        }

        public async Task<LessonPdfResponseDto> UploadPdfAsync(int lessonId, LessonPdfUploadRequestDto request)
        {
            if (request.File == null || request.File.Length == 0)
                throw new BadRequestException("PDF file is empty or null.");

            var extension = Path.GetExtension(request.File.FileName).ToLowerInvariant();
            if (extension != ".pdf")
                throw new BadRequestException("Only PDF files are allowed.");

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var s3Key = $"lessons/{lessonId}/pdfs/{uniqueFileName}";

            var url = await _fileUploader.UploadFileAsync(request.File, s3Key);

            var lessonPdf = new LessonPdf
            {
                Title = request.Title,
                S3Key = s3Key,
                Url = url,
                UploadedAt = DateTime.UtcNow,
                LessonId = lessonId
            };

            await _unitOfWork.LessonPdfs.AddAsync(lessonPdf);
            await _unitOfWork.CompleteAsync();

            return MapToDto(lessonPdf);
        }

        public async Task DeletePdfAsync(int lessonPdfId)
        {
            var pdf = await _unitOfWork.LessonPdfs.GetByIdAsync(lessonPdfId);
            if (pdf == null)
                throw new NotFoundException($"PDF with ID {lessonPdfId} not found.");

            await _fileUploader.DeleteFileAsync(pdf.S3Key);
            await _unitOfWork.LessonPdfs.DeleteAsync(pdf);
            await _unitOfWork.CompleteAsync();
        }

        private static LessonPdfResponseDto MapToDto(LessonPdf pdf) => new()
        {
            LessonPdfId = pdf.LessonPdfId,
            Title = pdf.Title,
            Url = pdf.Url,
            UploadedAt = pdf.UploadedAt
        };
    }
}
