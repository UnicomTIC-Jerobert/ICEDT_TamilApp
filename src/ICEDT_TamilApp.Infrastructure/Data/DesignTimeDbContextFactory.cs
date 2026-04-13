using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ICEDT_TamilApp.Infrastructure.Data
{
    /// <summary>
    /// Used only by EF Core CLI tools at design time (migrations, scaffolding).
    /// Not used at runtime.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlite("Data Source=tamilapp.db");
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
