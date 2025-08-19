using ICEDT_TamilApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICEDT_TamilApp.Infrastructure.Data.EntityConfigurations
{
    public class ActivityTypeConfiguration : IEntityTypeConfiguration<ActivityType>
    {
        public void Configure(EntityTypeBuilder<ActivityType> builder)
        {
            builder.HasKey(t => t.ActivityTypeId);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(50);

            builder
                .HasMany(t => t.Activities)
                .WithOne(a => a.ActivityType)
                .HasForeignKey(a => a.ActivityTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new ActivityType { ActivityTypeId = 1, Name = "PronunciationPractice" },
                    new ActivityType { ActivityTypeId = 2, Name = "AudioImageRecognition" },
                    new ActivityType { ActivityTypeId = 3, Name = "Dictation" },
                    new ActivityType { ActivityTypeId = 4, Name = "Matching" },
                    new ActivityType { ActivityTypeId = 5, Name = "SortingAndClassification" },
                    new ActivityType { ActivityTypeId = 6, Name = "OddOneOut" },
                    new ActivityType { ActivityTypeId = 7, Name = "FillInTheBlanks" },
                    new ActivityType { ActivityTypeId = 8, Name = "WordScramble" },
                    new ActivityType { ActivityTypeId = 9, Name = "SentenceScramble" },
                    new ActivityType { ActivityTypeId = 10, Name = "WordFormation" },
                    new ActivityType { ActivityTypeId = 11, Name = "SentenceBuilding" },
                    new ActivityType { ActivityTypeId = 12, Name = "GrammarPuzzle" },
                    new ActivityType { ActivityTypeId = 13, Name = "MultipleChoiceQuestion" },
                    new ActivityType { ActivityTypeId = 14, Name = "TrueOrFalse" },
                    new ActivityType { ActivityTypeId = 15, Name = "ReadingComprehension" },
                    new ActivityType { ActivityTypeId = 16, Name = "StorySequencing" },
                    new ActivityType { ActivityTypeId = 17, Name = "TimedChallenge" },
                    new ActivityType { ActivityTypeId = 18, Name = "InteractiveDialogue" }
            );
        }
    }
}
