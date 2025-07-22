
using ICEDT_TamilApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace 
ICEDT_TamilApp.Infrastructure.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Level> Levels { get; set; }
        DbSet<Lesson> Lessons { get; set; }
        DbSet<Activity> Activities { get; set; }
        DbSet<ActivityType> ActivityTypes { get; set; }
        DbSet<MainActivityType> MainActivityTypes { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
} 