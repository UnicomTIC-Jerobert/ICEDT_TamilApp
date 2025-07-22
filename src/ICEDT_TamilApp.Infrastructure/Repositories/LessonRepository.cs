using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ICEDT.API.Models;
using 
ICEDT_TamilApp.Infrastructure.Data;
using ICEDT.API.Repositories.Interfaces;

namespace 
ICEDT_TamilApp.Infrastructure.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly IApplicationDbContext _context;

        public LessonRepository(IApplicationDbContext context) => _context = context;

        public async Task<Level> GetByIdWithLessonsAsync(int id)
        {
            return await _context.Levels
                .Include(l => l.Lessons)
                .Where(l => l.LevelId == id)
                .OrderBy(l => l.SequenceOrder)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Level>> GetAllWithLessonsAsync()
        {
            var levels = await _context.Levels
                .Include(l => l.Lessons)
                .OrderBy(l => l.SequenceOrder)
                .ToListAsync();
            // Sort child lessons in-memory
            foreach (var level in levels)
            {
                if (level.Lessons != null)
                    level.Lessons = level.Lessons.OrderBy(ls => ls.SequenceOrder).ToList();
            }
            return levels;
        }

        public async Task<bool> SequenceOrderExistsAsync(int sequenceOrder) =>
            await _context.Levels.AnyAsync(l => l.SequenceOrder == sequenceOrder);

        public Task<Lesson?> GetByIdAsync(int lessonId)
        {
            throw new NotImplementedException();
        }

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
    }
}