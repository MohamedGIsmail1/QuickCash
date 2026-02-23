using QuickCash.Api.Dtos.Categories;

namespace QuickCash.Api.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto> CreateAsync(CreateCategoryRequest request);
    Task DeleteAsync(int id);
}
