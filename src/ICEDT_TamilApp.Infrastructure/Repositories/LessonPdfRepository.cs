using System.Collections.Generic;
using System.Threading.Tasks;
using ICEDT_TamilApp.Domain.Entities;
using ICEDT_TamilApp.Domain.Interfaces;
using ICEDT_TamilApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ICEDT_TamilApp.Infrastructure.Repositories
{
    public class LessonPdfRepository : ILessonPdfRepository
    {
        private readonly ApplicationDbContext _context;

        public LessonPdfRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<LessonPdf>> GetAllByLessonIdAsync(int lessonId)
        {
            return await _context.LessonPdfs
                .Where(p => p.LessonId == lessonId)
                .OrderBy(p => p.UploadedAt)
                .ToListAsync();
        }

        public async Task<LessonPdf?> GetByIdAsync(int lessonPdfId)
        {
            return await _context.LessonPdfs.FindAsync(lessonPdfId);
        }

        public async Task AddAsync(LessonPdf lessonPdf)
        {
            _context.LessonPdfs.Add(lessonPdf);
        }

        public async Task DeleteAsync(LessonPdf lessonPdf)
        {
            _context.LessonPdfs.Remove(lessonPdf);
        }
    }
}
