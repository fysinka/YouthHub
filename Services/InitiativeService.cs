
using Microsoft.EntityFrameworkCore;
using YouthHub.Data;
using YouthHub.Models;

namespace YouthHub.Services
{
    public class InitiativeService : IInitiativeService
    {
        private readonly ApplicationDbContext _context;

        public InitiativeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Initiative>> GetAllAsync()
        {
            return await _context.Initiatives
                .Include(i => i.Category)
                .OrderBy(i => i.EventDate)
                .ToListAsync();
        }
        public async Task<List<Initiative>> SearchAsync(
    string? searchString,
    int? categoryId)
{
    var query = _context.Initiatives
        .Include(i => i.Category)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(searchString))
    {
        query = query.Where(i =>
            i.Title.Contains(searchString) ||
            i.Description.Contains(searchString) ||
            i.Location.Contains(searchString));
    }

    if (categoryId.HasValue)
    {
        query = query.Where(i =>
            i.CategoryId == categoryId.Value);
    }

    return await query
        .OrderBy(i => i.EventDate)
        .ToListAsync();
}

        public async Task<Initiative?> GetByIdAsync(int id)
        {
            return await _context.Initiatives
                .Include(i => i.Category)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(Initiative initiative)
        {
            _context.Initiatives.Add(initiative);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Initiative initiative)
        {
            _context.Initiatives.Update(initiative);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Initiative initiative)
        {
            _context.Initiatives.Remove(initiative);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _context.Categories
                .AnyAsync(c => c.Id == categoryId);
        }
    }
}
