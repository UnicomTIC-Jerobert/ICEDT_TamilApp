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

            builder.HasData(
                new Level { LevelId = 1, LevelName = "மழலையர் நிலை", SequenceOrder = 1, Slug = "malalaiyar-nilai" },
                new Level { LevelId = 2, LevelName = "சிறுவர் நிலை", SequenceOrder = 2, Slug = "siruvar-nilai" },
                new Level { LevelId = 3, LevelName = "ஆண்டு 01", SequenceOrder = 3, Slug = "aandu-01" },
                new Level { LevelId = 4, LevelName = "ஆண்டு 02", SequenceOrder = 4, Slug = "aandu-02" },
                new Level { LevelId = 5, LevelName = "ஆண்டு 03", SequenceOrder = 5, Slug = "aandu-03" },
                new Level { LevelId = 6, LevelName = "ஆண்டு 04", SequenceOrder = 6, Slug = "aandu-04" },
                new Level { LevelId = 7, LevelName = "ஆண்டு 05", SequenceOrder = 7, Slug = "aandu-05" }
            );
        }


    }
}
