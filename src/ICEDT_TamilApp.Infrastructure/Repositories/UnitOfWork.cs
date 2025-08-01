using System.Threading.Tasks;
using ICEDT_TamilApp.Domain.Interfaces;
using ICEDT_TamilApp.Infrastructure.Data;

namespace ICEDT_TamilApp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IMainActivityRepository MainActivities { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            MainActivities = new MainActivityRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
