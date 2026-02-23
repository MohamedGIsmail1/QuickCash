using Microsoft.EntityFrameworkCore;
using QuickCash.Api.Data;
using QuickCash.Api.Models;
using QuickCash.Api.Repositories.Interfaces;

namespace QuickCash.Api.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly QuickCashDbContext _db;

    public CategoryRepository(QuickCashDbContext db)
    {
        _db = db;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _db.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _db.Categories
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category?> GetByNormalizedNameAsync(string normalizedName)
    {
        return await _db.Categories
            .FirstOrDefaultAsync(c => c.NormalizedName == normalizedName);
    }

    public async Task AddAsync(Category category)
    {
        await _db.Categories.AddAsync(category);
    }

    public Task DeleteAsync(Category category)
    {
        _db.Categories.Remove(category);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task<bool> IsCategoryInUseAsync(int categoryId)
    {
        return await _db.Transactions.AnyAsync(t => t.CategoryId == categoryId);
    }
}

