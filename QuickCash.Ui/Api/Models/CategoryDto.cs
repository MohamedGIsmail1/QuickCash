namespace QuickCash.Ui.Api.Models;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Type { get; set; } // 1=Income, 2=Expense 
}
