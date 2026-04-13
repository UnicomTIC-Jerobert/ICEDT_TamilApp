using ICEDT_TamilApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICEDT_TamilApp.Infrastructure.Data.EntityConfigurations
{
    public class LessonPdfConfiguration : IEntityTypeConfiguration<LessonPdf>
    {
        public void Configure(EntityTypeBuilder<LessonPdf> builder)
        {
            builder.HasKey(p => p.LessonPdfId);

            builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
            builder.Property(p => p.S3Key).IsRequired();
            builder.Property(p => p.Url).IsRequired();
            builder.Property(p => p.UploadedAt).IsRequired();

            builder
                .HasOne(p => p.Lesson)
                .WithMany()
                .HasForeignKey(p => p.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
