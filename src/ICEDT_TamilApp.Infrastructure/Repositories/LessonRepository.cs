using System.Collections.Generic;
using System.Threading.Tasks;
using ICEDT_TamilApp.Domain.Entities;
using ICEDT_TamilApp.Domain.Interfaces;
using ICEDT_TamilApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ICEDT_TamilApp.Infrastructure.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly ApplicationDbContext _context;

        public LessonRepository(ApplicationDbContext context) => _context = context;

        public async Task<Lesson> GetByIdAsync(int lessonId)
        {
            return await _context
                .Lessons.Where(l => l.LessonId == lessonId)
                .OrderBy(l => l.SequenceOrder)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Lesson>> GetAllAsync()
        {
            var lessons = await _context.Lessons.OrderBy(l => l.SequenceOrder).ToListAsync();

            return lessons;
        }

        public async Task<bool> SequenceOrderExistsAsync(int sequenceOrder) =>
            await _context.Levels.AnyAsync(l => l.SequenceOrder == sequenceOrder);

        public Task<Lesson> CreateAsync(Lesson lesson)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Lesson lesson)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int lessonId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Lesson>> GetAllLessonsByLevelIdAsync(int levelId)
        {
            var lessons = await _context
                .Lessons.Where(l => l.LevelId == levelId)
                .OrderBy(l => l.SequenceOrder)
                .ToListAsync();

            return lessons;
        }
    }
}
