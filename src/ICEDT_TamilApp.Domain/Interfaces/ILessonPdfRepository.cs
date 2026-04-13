using System.Collections.Generic;
using System.Threading.Tasks;
using ICEDT_TamilApp.Domain.Entities;

namespace ICEDT_TamilApp.Domain.Interfaces
{
    public interface ILessonPdfRepository
    {
        Task<List<LessonPdf>> GetAllByLessonIdAsync(int lessonId);
        Task<LessonPdf?> GetByIdAsync(int lessonPdfId);
        Task AddAsync(LessonPdf lessonPdf);
        Task DeleteAsync(LessonPdf lessonPdf);
    }
}
