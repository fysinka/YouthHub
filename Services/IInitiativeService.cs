
using YouthHub.Models;

namespace YouthHub.Services
{
    public interface IInitiativeService
    {
        Task<List<Initiative>> GetAllAsync();
        Task<List<Initiative>> SearchAsync(
            string? searchString,
            int? categoryId);
        Task<Initiative?> GetByIdAsync(int id);
        Task AddAsync(Initiative initiative);
        Task UpdateAsync(Initiative initiative);
        Task DeleteAsync(Initiative initiative);
        Task<bool> CategoryExistsAsync(int categoryId);
    }
}
