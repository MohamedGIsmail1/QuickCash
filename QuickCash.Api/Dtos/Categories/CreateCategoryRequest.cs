using QuickCash.Api.Models;

namespace QuickCash.Api.Dtos.Categories;

public class CreateCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
}
