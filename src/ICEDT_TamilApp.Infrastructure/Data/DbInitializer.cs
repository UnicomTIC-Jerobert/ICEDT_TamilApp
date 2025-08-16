using System;
using System.Linq;
using System.Threading.Tasks; // Add this for async operations
using ICEDT_TamilApp.Domain.Entities;
using Microsoft.EntityFrameworkCore; // Add this for async operations

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
                new Level { LevelName = "மழலையர் நிலை", SequenceOrder = 1,Slug="malayalar-nilai" },
                new Level { LevelName = "சிறுவர் நிலை", SequenceOrder = 2 ,Slug="siruvar-nilai"},
                new Level { LevelName = "ஆண்டு 01", SequenceOrder = 3 ,Slug="aandu-01"},
                new Level { LevelName = "ஆண்டு 02", SequenceOrder = 4,Slug="aandu-02" },
                new Level { LevelName = "ஆண்டு 03", SequenceOrder = 5 ,Slug="aandu-03"},
                new Level { LevelName = "ஆண்டு 04", SequenceOrder = 6 ,Slug="aandu-04"},
                new Level { LevelName = "ஆண்டு 05", SequenceOrder = 7 ,Slug="aandu-05"},
            };
            await context.Levels.AddRangeAsync(levels);
            await context.SaveChangesAsync(); // Use async version

            // --- Seed Lessons ---
            var aandu01LevelId = (
                await context.Levels.SingleAsync(l => l.SequenceOrder == 3)
            ).LevelId;
            var aandu02LevelId = (
                await context.Levels.SingleAsync(l => l.SequenceOrder == 4)
            ).LevelId;
            // ... get other level IDs as needed

            var lessons = new Lesson[]
            {
                new Lesson
                {
                    LevelId = aandu01LevelId,
                    LessonName = "பாடம் 01 - தமிழ்ப்பள்ளி",
                    SequenceOrder = 1,
                    Description = "Introduction to classroom vocabulary and basic alphabets.",
                },
                new Lesson
                {
                    LevelId = aandu01LevelId,
                    LessonName = "பாடம் 02 - விடுமுறை",
                    SequenceOrder = 2,
                    Description = "Learning the 'ஆ' sound series.",
                },
                // ... add all other lessons
            };
            await context.Lessons.AddRangeAsync(lessons);
            await context.SaveChangesAsync();

            // --- Seed MainActivities ---
            if (!await context.MainActivities.AnyAsync())
            {
                var mainActivities = new MainActivity[]
                {
                    new MainActivity { Name = "Video" },
                    new MainActivity { Name = "Sounds" },
                    new MainActivity { Name = "Learning" },
                    new MainActivity { Name = "Exercises" },
                };
                await context.MainActivities.AddRangeAsync(mainActivities);
                await context.SaveChangesAsync();
            }

            // --- Seed ActivityTypes ---
            if (!await context.ActivityTypes.AnyAsync())
            {
                var activityTypes = new ActivityType[]
                {
                    new ActivityType { Name = "PronunciationPractice" },
                    new ActivityType { Name = "AudioImageRecognition" },
                    new ActivityType { Name = "Dictation" },
                    new ActivityType { Name = "Matching" },
                    new ActivityType { Name = "SortingAndClassification" },
                    new ActivityType { Name = "OddOneOut" },
                    new ActivityType { Name = "FillInTheBlanks" },
                    new ActivityType { Name = "WordScramble" },
                    new ActivityType { Name = "SentenceScramble" },
                    new ActivityType { Name = "WordFormation" },
                    new ActivityType { Name = "SentenceBuilding" },
                    new ActivityType { Name = "GrammarPuzzle" },
                    new ActivityType { Name = "MultipleChoiceQuestion" },
                    new ActivityType { Name = "TrueOrFalse" },
                    new ActivityType { Name = "ReadingComprehension" },
                    new ActivityType { Name = "StorySequencing" },
                    new ActivityType { Name = "TimedChallenge" },
                    new ActivityType { Name = "InteractiveDialogue" },
                };
                await context.ActivityTypes.AddRangeAsync(activityTypes);
                await context.SaveChangesAsync();
            }

            // Inside the Initialize method, after seeding Lessons...

            // --- Seed placeholder Activities ---
            if (!await context.Activities.AnyAsync())
            {
                var aandu01Lesson1Id = (await context.Lessons.FirstAsync(l => l.Level.SequenceOrder == 3 && l.SequenceOrder == 1)).LessonId;
                // First, get the LessonId for Grade 1, Lesson 5
                var aandu01Lesson5Id = (await context.Lessons.FirstAsync(l => l.Level.SequenceOrder == 3 && l.SequenceOrder == 5)).LessonId;

                var activities = new Activity[]
                {
        // Activity from Grade 1, Lesson 1, Exercise 06
        // ...
        new Activity
        {
            LessonId = aandu01Lesson1Id,
            Title = "அ-ஓசை உயிர்மெய் எழுத்துகள்",
            SequenceOrder = 1,
            ActivityTypeId = 7, // FillInTheBlanks
            MainActivityId = 3,
            ContentJson = @"{
              ""leftOperand"": ""க்"",
              ""rightOperand"": ""அ"",
              ""correctAnswer"": ""க"",
              ""options"": [""கா"", ""கி"", ""க"", ""கூ""]
            }"
        },
        new Activity
        {
            LessonId = aandu01Lesson1Id,
            Title = "அ-ஓசை உயிர்மெய் எழுத்துகள்",
            SequenceOrder = 2,
            ActivityTypeId = 7,
            MainActivityId = 4,
            ContentJson = @"{
              ""leftOperand"": ""ச்"",
              ""rightOperand"": ""அ"",
              ""correctAnswer"": ""ச"",
              ""options"": [""சா"", ""சி"", ""ச"", ""சூ""]
            }"
        },
        new Activity
        {
            LessonId = aandu01Lesson1Id,
            Title = "முதல் எழுத்துச் சொல் கண்டறிதல்",
            SequenceOrder = 3, // Or whatever sequence is appropriate
            ActivityTypeId = 4, // Matching
            MainActivityId = 4, // Exercises
            ContentJson = @"{
            ""title"": ""Find the word that starts with the letter shown above."",
            ""words"": [""பல்"", ""கல்"", ""கண்"", ""மண்"", ""வயல்"", ""மரம்"", ""படம்"", ""தடம்"", ""அப்பம்"", ""மன்னன்""]
            }"
        },
        // Inside the Initialize method...



// --- Add these new activities to the array ---

new Activity
{
    LessonId = aandu01Lesson5Id,
    Title = "எண்களை எழுத்துக்களுடன் பொருத்துக (1-5)",
    SequenceOrder = 1,
    ActivityTypeId = 4, // Matching
    MainActivityId = 4, // Exercises
    ContentJson = @"{
      ""title"": ""Match the Number to the Word"",
      ""columnA"": [
        { ""id"": ""A1"", ""content"": ""1"", ""matchId"": ""B1"" },
        { ""id"": ""A2"", ""content"": ""2"", ""matchId"": ""B2"" },
        { ""id"": ""A3"", ""content"": ""3"", ""matchId"": ""B3"" },
        { ""id"": ""A4"", ""content"": ""4"", ""matchId"": ""B4"" },
        { ""id"": ""A5"", ""content"": ""5"", ""matchId"": ""B5"" }
      ],
      ""columnB"": [
        { ""id"": ""B1"", ""content"": ""ஒன்று"", ""matchId"": ""A1"" },
        { ""id"": ""B2"", ""content"": ""இரண்டு"", ""matchId"": ""A2"" },
        { ""id"": ""B3"", ""content"": ""மூன்று"", ""matchId"": ""A3"" },
        { ""id"": ""B4"", ""content"": ""நான்கு"", ""matchId"": ""A4"" },
        { ""id"": ""B5"", ""content"": ""ஐந்து"", ""matchId"": ""A5"" }
      ]
    }"
},
new Activity
{
    LessonId = aandu01Lesson5Id,
    Title = "எண்களை எழுத்துக்களுடன் பொருத்துக (6-10)",
    SequenceOrder = 2,
    ActivityTypeId = 4, // Matching
    MainActivityId = 4, // Exercises
    ContentJson = @"{
      ""title"": ""Match the Number to the Word"",
      ""columnA"": [
        { ""id"": ""A6"", ""content"": ""6"", ""matchId"": ""B6"" },
        { ""id"": ""A7"", ""content"": ""7"", ""matchId"": ""B7"" },
        { ""id"": ""A8"", ""content"": ""8"", ""matchId"": ""B8"" },
        { ""id"": ""A9"", ""content"": ""9"", ""matchId"": ""B9"" },
        { ""id"": ""A10"", ""content"": ""10"", ""matchId"": ""B10"" }
      ],
      ""columnB"": [
        { ""id"": ""B6"", ""content"": ""ஆறு"", ""matchId"": ""A6"" },
        { ""id"": ""B7"", ""content"": ""ஏழு"", ""matchId"": ""B7"" },
        { ""id"": ""B8"", ""content"": ""எட்டு"", ""matchId"": ""B8"" },
        { ""id"": ""B9"", ""content"": ""ஒன்பது"", ""matchId"": ""B9"" },
        { ""id"": ""B10"", ""content"": ""பத்து"", ""matchId"": ""B10"" }
      ]
    }"
}


                };

                await context.Activities.AddRangeAsync(activities);
                await context.SaveChangesAsync();
            }
        }
    }
}
