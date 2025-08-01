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

            builder.HasIndex(a => new { a.LessonId, a.SequenceOrder }).IsUnique();
        }
    }
}
