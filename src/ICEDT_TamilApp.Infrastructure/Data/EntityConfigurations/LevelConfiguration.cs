using ICEDT_TamilApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICEDT_TamilApp.Infrastructure.Data.EntityConfigurations
{
    public class LevelConfiguration : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> builder)
        {
            builder.HasKey(l => l.LevelId);
            builder.Property(l => l.LevelName).IsRequired();
            builder.Property(l => l.SequenceOrder).IsRequired();

            builder
                .HasMany(l => l.Lessons)
                .WithOne(ls => ls.Level)
                .HasForeignKey(ls => ls.LevelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(l => l.SequenceOrder).IsUnique();
        }
    }
}
