namespace QuickCash.Ui.Api.Models;

public class CreateTransactionRequest
{
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public int CategoryId { get; set; }
    public string? Note { get; set; }
}
