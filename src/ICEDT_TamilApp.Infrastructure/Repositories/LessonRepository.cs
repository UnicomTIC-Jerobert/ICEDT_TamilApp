using System.Collections.Generic;
using System.Linq; // Required for .Where()
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
            // No changes needed here, this is fine.
            return await _context
                .Lessons.AsNoTracking() // Use AsNoTracking for read-only queries
                .FirstOrDefaultAsync(l => l.LessonId == lessonId);
        }

        public async Task<List<Lesson>> GetAllAsync()
        {
            // No changes needed here, this is fine.
            return await _context
                .Lessons.AsNoTracking()
                .OrderBy(l => l.SequenceOrder)
                .ToListAsync();
        }

        public async Task<List<Lesson>> GetAllLessonsByLevelIdAsync(int levelId)
        {
            // No changes needed here, this is fine.
            return await _context
                .Lessons.AsNoTracking()
                .Where(l => l.LevelId == levelId)
                .OrderBy(l => l.SequenceOrder)
                .ToListAsync();
        }

        // --- FIX 1: Implement CreateAsync ---
        public async Task<Lesson> CreateAsync(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return lesson; // The lesson object now has the new ID from the database
        }

        // --- FIX 2: Implement UpdateAsync ---
        public async Task<bool> UpdateAsync(Lesson lesson)
        {
            // Find the existing lesson in the database
            var existingLesson = await _context.Lessons.FindAsync(lesson.LessonId);
            if (existingLesson == null)
            {
                return false; // Indicate that the lesson was not found
            }

            // Update the tracked entity's values. This is more efficient than Attach/Modify.
            _context.Entry(existingLesson).CurrentValues.SetValues(lesson);

            await _context.SaveChangesAsync();
            return true;
        }

        // --- FIX 3: Implement DeleteAsync ---
        public async Task<bool> DeleteAsync(int lessonId)
        {
            var lessonToDelete = await _context.Lessons.FindAsync(lessonId);
            if (lessonToDelete == null)
            {
                return false; // The lesson to delete was not found
            }

            _context.Lessons.Remove(lessonToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SequenceOrderExistsAsync(int sequenceOrder) =>
            await _context.Lessons.AnyAsync(l => l.SequenceOrder == sequenceOrder); // Corrected to check Lessons table
    }
}
