using ICEDT_TamilApp.Domain.Entities;
using ICEDT_TamilApp.Domain.Interfaces;
using ICEDT_TamilApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ICEDT_TamilApp.Infrastructure.Repositories
{
    public class ActivityTypeRepository : IActivityTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityTypeRepository(ApplicationDbContext context) => _context = context;

        public async Task<ActivityType> GetByIdAsync(int id) =>
            await _context.ActivityTypes.FindAsync(id);

        public async Task<List<ActivityType>> GetAllAsync() =>
            await _context.ActivityTypes.ToListAsync();

        public async Task CreateAsync(ActivityType activityType)
        {
            _context.ActivityTypes.Add(activityType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ActivityType activityType)
        {
            _context.ActivityTypes.Update(activityType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var activityType = await _context.ActivityTypes.FindAsync(id);
            if (activityType != null)
            {
                _context.ActivityTypes.Remove(activityType);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ActivityTypeExistsAsync(int activityTypeId) =>
            await _context.ActivityTypes.AnyAsync(t => t.ActivityTypeId == activityTypeId);
    }
}
