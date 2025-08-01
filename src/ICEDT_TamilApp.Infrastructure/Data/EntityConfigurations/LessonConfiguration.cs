using ICEDT_TamilApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICEDT_TamilApp.Infrastructure.Data.EntityConfigurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(ls => ls.LessonId);
            builder.Property(ls => ls.LessonName).IsRequired();
            builder.Property(ls => ls.Description).IsRequired();
            builder.Property(ls => ls.SequenceOrder).IsRequired();

            builder
                .HasOne(ls => ls.Level)
                .WithMany(l => l.Lessons)
                .HasForeignKey(ls => ls.LevelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(ls => ls.Activities)
                .WithOne(a => a.Lesson)
                .HasForeignKey(a => a.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(l => new { l.LevelId, l.SequenceOrder }).IsUnique();
        }
    }
}
