using System.Collections.Generic;
using System.Threading.Tasks;
using ICEDT_TamilApp.Domain.Entities;

namespace ICEDT_TamilApp.Domain.Interfaces
{
    public interface IMainActivityTypeRepository
    {
        Task<MainActivityType> GetByIdAsync(int mainActivityTypeId);
        Task<List<MainActivityType>> GetAllAsync();
        Task CreateAsync(MainActivityType activityType);
        Task UpdateAsync(MainActivityType activityType);
        Task DeleteAsync(int id);
        Task<bool> MainActivityTypeExistsAsync(int mainActivityTypeId);
    }
}