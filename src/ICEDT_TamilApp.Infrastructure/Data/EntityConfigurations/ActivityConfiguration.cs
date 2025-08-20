using ICEDT_TamilApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICEDT_TamilApp.Infrastructure.Data.EntityConfigurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(a => a.ActivityId);
            builder.Property(a => a.LessonId).IsRequired();
            builder.Property(a => a.ActivityTypeId).IsRequired();
            builder.Property(a => a.Title).IsRequired().HasMaxLength(100);
            builder.Property(a => a.SequenceOrder).IsRequired();
            builder.Property(a => a.ContentJson).IsRequired();

            builder
                .HasOne(a => a.Lesson)
                .WithMany(l => l.Activities)
                .HasForeignKey(a => a.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(a => a.ActivityType)
                .WithMany(t => t.Activities)
                .HasForeignKey(a => a.ActivityTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(a => a.MainActivity)
                .WithMany(m => m.Activities)
                .HasForeignKey(a => a.MainActivityId)
                .IsRequired();

            builder.HasIndex(a => new { a.LessonId,a.SequenceOrder }).IsUnique();
            
            builder.HasData(
                
                // === Activity for Grade 1, Lesson 1: Fill in the Equation (ID: 7) ===
                new Activity
                {
                    ActivityId = 1,      // Must provide a unique PK
                    LessonId = 21,       // Belongs to "ஆண்டு 01 - பாடம் 01"
                    Title = "அ-ஓசை உயிர்மெய் எழுத்துகள் (க் + அ)",
                    SequenceOrder = 1,
                    ActivityTypeId = 7,  // ID for FillInTheBlanks
                    MainActivityId = 3,  // ID for "Learning"
                    ContentJson = @"{
                      ""leftOperand"": ""க்"",
                      ""rightOperand"": ""அ"",
                      ""correctAnswer"": ""க"",
                      ""options"": [""கா"", ""கி"", ""க"", ""கூ""]
                    }"
                },

                // === Activity for Grade 1, Lesson 1: First Letter Match (ID: 4) ===
                new Activity
                {
                    ActivityId = 2,
                    LessonId = 21,
                    Title = "முதல் எழுத்துச் சொல் கண்டறிதல்",
                    SequenceOrder = 2,
                    ActivityTypeId = 4, // ID for Matching
                    MainActivityId = 4, // ID for "Exercises"
                    ContentJson = @"{
                      ""title"": ""Find the word that starts with the letter shown above."",
                      ""words"": [""பல்"", ""கல்"", ""கண்"", ""மண்"", ""வயல்"", ""மரம்"", ""படம்"", ""தடம்"", ""அப்பம்"", ""மன்னன்""]
                    }"
                },

                // === Activity for Grade 1, Lesson 5: Number Matching (ID: 4) ===
                new Activity
                {
                    ActivityId = 3,
                    LessonId = 25, // Belongs to "ஆண்டு 01 - பாடம் 05"
                    Title = "எண்களை எழுத்துக்களுடன் பொருத்துக (1-5)",
                    SequenceOrder = 1,
                    ActivityTypeId = 4, // ID for Matching
                    MainActivityId = 4, // ID for "Exercises"
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

                // === Activity for Grade 2, Lesson 2: Word Bank Completion (ID: 7) ===
                new Activity
                {
                    ActivityId = 4,
                    LessonId = 32, // Belongs to "ஆண்டு 02 - பாடம் 02"
                    Title = "வினைச்சொல் பொருத்தம் (Word Bank)",
                    SequenceOrder = 1,
                    ActivityTypeId = 7, // ID for FillInTheBlanks
                    MainActivityId = 4, // ID for "Exercises"
                    ContentJson = @"{
                      ""title"": ""Fill in the blanks with the correct word from the word bank."",
                      ""sentences"": [
                        { ""id"": 1, ""prefix"": """", ""suffix"": "" செழிப்பாக வளர்கின்றன."", ""correctAnswer"": ""அவை"" },
                        { ""id"": 2, ""prefix"": """", ""suffix"": "" பேருந்தில் செல்கிறாள்."", ""correctAnswer"": ""அவள்"" }
                      ],
                      ""wordBank"": [""அவை"", ""அவள்"", ""பறக்கிறது"", ""ஆடுகிறோம்""]
                    }"
                },

                // === Activity for Grade 3, Lesson 3: Dropdown Completion (ID: 7) ===
                new Activity
                {
                    ActivityId = 5,
                    LessonId = 45, // Belongs to "ஆண்டு 03 - பாடம் 03"
                    Title = "பொருத்தமான வினைச்சொல்லைத் தெரிவு செய்க (Dropdown)",
                    SequenceOrder = 1,
                    ActivityTypeId = 7, // ID for FillInTheBlanks
                    MainActivityId = 4, // ID for "Exercises"
                    ContentJson = @"{
                      ""title"": ""Complete the sentences by selecting the correct verb."",
                      ""sentences"": [
                        {
                          ""id"": 1,
                          ""prefix"": ""மாதவி"",
                          ""suffix"": ""பாட்டு."",
                          ""options"": [""பாடுகிறாள்"", ""வரைகிறான்"", ""மீட்டுகிறாள்""],
                          ""correctAnswer"": ""பாடுகிறாள்""
                        },
                        {
                          ""id"": 2,
                          ""prefix"": ""மகிழன்"",
                          ""suffix"": ""ஓவியம்."",
                          ""options"": [""ஆடுகிறாள்"", ""வரைகிறான்"", ""ஊதுகிறாள்""],
                          ""correctAnswer"": ""வரைகிறான்""
                        }
                      ]
                    }"
                },
                
                // === A Sample MCQ Activity (ID: 13) ===
                 new Activity
                {
                    ActivityId = 6,
                    LessonId = 1, // Belongs to a Preschool lesson
                    Title = "உடல் உறுப்புகள் கேள்வி",
                    SequenceOrder = 1,
                    ActivityTypeId = 13, // ID for MCQ
                    MainActivityId = 4, // ID for "Exercises"
                    ContentJson = @"{
                      ""question"": ""Which part of the body do we use to see?"",
                      ""choices"": [
                        { ""id"": 1, ""text"": ""காது (Ear)"", ""isCorrect"": false },
                        { ""id"": 2, ""text"": ""கண் (Eye)"", ""isCorrect"": true },
                        { ""id"": 3, ""text"": ""மூக்கு (Nose)"", ""isCorrect"": false }
                      ]
                    }"
                }
            );
        }
    }
}