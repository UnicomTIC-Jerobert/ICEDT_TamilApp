using System.Threading.Tasks;

namespace ICEDT_TamilApp.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMainActivityRepository MainActivities { get; }
        // Add other repositories here as you create them
        // IActivityTypeRepository ActivityTypes { get; }
        Task<int> CompleteAsync();
    }
}