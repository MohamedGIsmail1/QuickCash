using QuickCash.Api.Models;

namespace QuickCash.Api.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);

    // For case-insensitive uniqueness checks (we compare on NormalizedName).
    Task<Category?> GetByNormalizedNameAsync(string normalizedName);

    // Repositories/Interfaces/ICategoryRepository.cs
    Task<bool> IsCategoryInUseAsync(int categoryId);

    Task AddAsync(Category category);
    Task DeleteAsync(Category category);
    Task SaveChangesAsync();
}
