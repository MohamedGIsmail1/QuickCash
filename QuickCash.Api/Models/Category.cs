namespace QuickCash.Api.Models;

public class Category
{
    public int Id { get; set; }

    // What the user sees and enters.
    public string Name { get; set; } = string.Empty;

    // Used for case-insensitive uniqueness (e.g., "Food" and "food" should be treated the same).
    public string NormalizedName { get; set; } = string.Empty;

    public CategoryType Type { get; set; }
}
