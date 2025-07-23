using System.Collections.Generic;
using System.Threading.Tasks;
using ICEDT_TamilApp.Domain.Entities;

namespace ICEDT_TamilApp.Domain.Interfaces
{
    public interface ILevelRepository
    {
        Task<Level> GetByIdAsync(int id);
        Task<List<Level>> GetAllAsync();
        Task CreateAsync(Level level);
        Task UpdateAsync(Level level);
        Task DeleteAsync(int id);

        Task<bool> SequenceOrderExistsAsync(int sequenceOrder);
    }
}
