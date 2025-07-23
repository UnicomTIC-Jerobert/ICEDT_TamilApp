
using ICEDT_TamilApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace
ICEDT_TamilApp.Infrastructure.Data.EntityConfigurations
{
    public class MainActivityConfiguration : IEntityTypeConfiguration<MainActivity>
    {
        public void Configure(EntityTypeBuilder<MainActivity> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Name).IsRequired().HasMaxLength(50);

            // Activities linked via ActivityType (no direct foreign key)
            builder.HasMany(m => m.Activities)
                   .WithOne()
                   .HasForeignKey(a => a.ActivityTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

                   
        }
    }
}