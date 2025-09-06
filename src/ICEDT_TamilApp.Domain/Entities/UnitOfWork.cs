namespace ICEDT_TamilApp.Domain.Entities
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IMainActivityRepository MainActivities { get; }
        public ILevelRepository Levels { get; }
        public ILessonRepository Lessons { get; }

        public IActivityTypeRepository ActivityTypes { get; }
        public IActivityRepository Activities { get; }

        public IUserRepository Users { get; private set; }

        // Add other repositories here as you create them
        // IActivityTypeRepository ActivityTypes { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IMainActivityRepository mainActivities,
            ILevelRepository levels,
            ILessonRepository lessons,
            IActivityTypeRepository activityTypes,
            IActivityRepository activities,
            IUserRepository users
        )
        {
            _context = context;
            MainActivities = mainActivities;
            Levels = levels;
            Lessons = lessons;
            ActivityTypes = activityTypes;
            Activities = activities;
            Users = users;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
