using System.Collections.Generic;
using System.Threading.Tasks;
using ICEDT_TamilApp.Domain.Entities;

namespace ICEDT_TamilApp.Domain.Interfaces
{
    public interface ILessonRepository
    {
        Task<Lesson?> GetByIdAsync(int lessonId);
        Task<List<Lesson>> GetAllAsync();
        Task<Lesson> CreateAsync(Lesson lesson);
        Task<bool> UpdateAsync(Lesson lesson);
        Task<bool> DeleteAsync(int lessonId);
        Task<List<Lesson>> GetAllLessonsByLevelIdAsync(int levelId);
    }
}