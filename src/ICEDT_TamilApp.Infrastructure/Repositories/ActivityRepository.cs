using System.Collections.Generic;
using System.Threading.Tasks;
using ICEDT_TamilApp.Domain.Entities;
using ICEDT_TamilApp.Domain.Interfaces;
using ICEDT_TamilApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ICEDT_TamilApp.Infrastructure.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityRepository(ApplicationDbContext context) => _context = context;

        public async Task<Activity> GetByIdAsync(int id) =>
            await _context
                .Activities.Include(a => a.ActivityType)
                .FirstOrDefaultAsync(a => a.ActivityId == id);

        public async Task<List<Activity>> GetAllAsync() =>
            await _context
                .Activities.Include(a => a.ActivityType)
                .OrderBy(a => a.SequenceOrder)
                .ToListAsync();

        public async Task<List<Activity>> GetByLessonIdAsync(int lessonId)
        {
            var query = _context
                .Activities.Include(a => a.ActivityType)
                .Where(a => a.LessonId == lessonId);

            return await query.OrderBy(a => a.SequenceOrder).ToListAsync();
        }

        public async Task CreateAsync(Activity activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Activity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity != null)
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SequenceOrderExistsAsync(int sequenceOrder) =>
            await _context.Activities.AnyAsync(a => a.SequenceOrder == sequenceOrder);

        public async Task<bool> LessonExistsAsync(int lessonId) =>
            await _context.Lessons.AnyAsync(l => l.LessonId == lessonId);
    }
}
