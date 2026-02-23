using QuickCash.Api.Dtos.Categories;
using QuickCash.Api.Models;
using QuickCash.Api.Repositories.Interfaces;
using QuickCash.Api.Services.Interfaces;

namespace QuickCash.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categories;

    public CategoryService(ICategoryRepository categories)
    {
        _categories = categories;
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var entities = await _categories.GetAllAsync();

        return entities
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type
            })
            .ToList();
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request)
    {
        var name = (request.Name ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.");

        // Type must be selected (enum default = 0). We only allow 1 or 2.
        if (!Enum.IsDefined(typeof(CategoryType), request.Type))
            throw new ArgumentException("Category type is required.");

        var normalized = name.ToUpperInvariant();

        var existing = await _categories.GetByNormalizedNameAsync(normalized);
        if (existing != null)
            throw new ArgumentException("Category name must be unique (case-insensitive).");

        var entity = new Category
        {
            Name = name,
            NormalizedName = normalized,
            Type = request.Type
        };

        await _categories.AddAsync(entity);
        await _categories.SaveChangesAsync();

        return new CategoryDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Type = entity.Type
        };
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _categories.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("Category not found.");

        var inUse = await _categories.IsCategoryInUseAsync(id);
        if (inUse)
            throw new InvalidOperationException("Category is in use and cannot be deleted.");

        await _categories.DeleteAsync(entity);
        await _categories.SaveChangesAsync();
    }

}
