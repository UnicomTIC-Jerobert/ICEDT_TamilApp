
using ICEDT_TamilApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace
ICEDT_TamilApp.Infrastructure.Data.EntityConfigurations
{
    public class MainActivityTypeConfiguration : IEntityTypeConfiguration<MainActivityType>
    {
        public void Configure(EntityTypeBuilder<MainActivityType> builder)
        {
            builder.HasKey(m => m.MainActivityTypeId);
            builder.Property(m => m.Name).IsRequired().HasMaxLength(50);

            // Activities linked via ActivityType (no direct foreign key)
            builder.HasMany(m => m.Activities)
                   .WithOne()
                   .HasForeignKey(a => a.ActivityTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}