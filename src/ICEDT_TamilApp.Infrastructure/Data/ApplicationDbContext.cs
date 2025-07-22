using ICEDT_TamilApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace 
ICEDT_TamilApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Level> Levels { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<MainActivityType> MainActivityTypes { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserCurrentProgress> UserCurrentProgress { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // ... existing configurations

            // Configure one-to-one relationship for UserCurrentProgress
            modelBuilder.Entity<UserCurrentProgress>()
                .HasKey(ucp => ucp.UserId); // UserId is both PK and FK

            // modelBuilder.Entity<User>()
            //     .HasOne(u => u.UserCurrentProgress) // Add navigation property to User model
            //     .WithOne(ucp => ucp.User)
            //     .HasForeignKey<UserCurrentProgress>(ucp => ucp.UserId);
        }
    }
}