using System.Collections.Generic;
using System.Threading.Tasks;
using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using ICEDT_TamilApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICEDT_TamilApp.Web.Controllers
{
    [ApiController]
    [Route("api/lessons/{lessonId}/pdfs")]
    [Authorize]
    public class LessonPdfsController : ControllerBase
    {
        private readonly ILessonPdfService _lessonPdfService;

        public LessonPdfsController(ILessonPdfService lessonPdfService)
        {
            _lessonPdfService = lessonPdfService;
        }

        /// <summary>
        /// Returns all PDFs for a given lesson. Accessible by any authenticated user.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<LessonPdfResponseDto>), 200)]
        public async Task<IActionResult> GetPdfs(int lessonId)
        {
            var pdfs = await _lessonPdfService.GetPdfsForLessonAsync(lessonId);
            return Ok(pdfs);
        }

        /// <summary>
        /// Uploads a PDF for a given lesson. Admin only.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(LessonPdfResponseDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UploadPdf(int lessonId, [FromForm] LessonPdfUploadRequestDto request)
        {
            var result = await _lessonPdfService.UploadPdfAsync(lessonId, request);
            return CreatedAtAction(nameof(GetPdfs), new { lessonId }, result);
        }

        /// <summary>
        /// Deletes a PDF by its ID. Admin only.
        /// </summary>
        [HttpDelete("{lessonPdfId}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePdf(int lessonId, int lessonPdfId)
        {
            await _lessonPdfService.DeletePdfAsync(lessonPdfId);
            return NoContent();
        }
    }
}
