using ICEDT_TamilApp.Domain.Entities;
using Microsoft.EntityFrameworkCore; // Add this for async operations
using System;
using System.Linq;
using System.Threading.Tasks; // Add this for async operations

namespace ICEDT_TamilApp.Infrastructure.Data
{
    public static class DbInitializer
    {
        // Change the method to be async and return a Task
        public static async Task Initialize(ApplicationDbContext context)
        {
            // Ensure the database is created. In development, this will create the DB.
            // In production, migrations are preferred.
            await context.Database.EnsureCreatedAsync();

            // Look for any levels. If they exist, the DB has been seeded.
            if (await context.Levels.AnyAsync())
            {
                return; // DB has been seeded
            }

            // --- Seed Levels ---
            var levels = new Level[]
            {
                new Level { LevelName = "மழலையர் நிலை", SequenceOrder = 1 },
                new Level { LevelName = "சிறுவர் நிலை", SequenceOrder = 2 },
                new Level { LevelName = "ஆண்டு 01", SequenceOrder = 3 },
                new Level { LevelName = "ஆண்டு 02", SequenceOrder = 4 },
                new Level { LevelName = "ஆண்டு 03", SequenceOrder = 5 },
                new Level { LevelName = "ஆண்டு 04", SequenceOrder = 6 },
                new Level { LevelName = "ஆண்டு 05", SequenceOrder = 7 }
            };
            await context.Levels.AddRangeAsync(levels);
            await context.SaveChangesAsync(); // Use async version

            // --- Seed Lessons ---
            var aandu01LevelId = (await context.Levels.SingleAsync(l => l.SequenceOrder == 3)).LevelId;
            var aandu02LevelId = (await context.Levels.SingleAsync(l => l.SequenceOrder == 4)).LevelId;
            // ... get other level IDs as needed

            var lessons = new Lesson[]
            {
                new Lesson { LevelId = aandu01LevelId, LessonName = "பாடம் 01 - தமிழ்ப்பள்ளி", SequenceOrder = 1, Description = "Introduction to classroom vocabulary and basic alphabets." },
                new Lesson { LevelId = aandu01LevelId, LessonName = "பாடம் 02 - விடுமுறை", SequenceOrder = 2, Description = "Learning the 'ஆ' sound series." },
                // ... add all other lessons
            };
            await context.Lessons.AddRangeAsync(lessons);
            await context.SaveChangesAsync();

            // --- Seed ActivityTypes ---
            if (!await context.ActivityTypes.AnyAsync())
            {
                // Your activity type seeding logic here...
            }
            
            // --- Seed placeholder Activities ---
            // Your activity seeding logic here...
        }
    }
}