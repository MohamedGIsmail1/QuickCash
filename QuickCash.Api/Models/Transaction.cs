namespace QuickCash.Api.Models;

public class Transaction
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    // Always stored as a positive number (business rule enforced in the Service layer later).
    public decimal Amount { get; set; }

    public int CategoryId { get; set; }

    public string? Note { get; set; }

    // Navigation property (lets EF join Category data when needed)
    public Category? Category { get; set; }
}
