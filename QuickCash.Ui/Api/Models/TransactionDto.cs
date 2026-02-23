namespace QuickCash.Ui.Api.Models;

public class TransactionDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public int CategoryType { get; set; }

    public string? Note { get; set; }
}
