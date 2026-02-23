using QuickCash.Api.Dtos.Overview;
using QuickCash.Api.Dtos.Transactions;
using QuickCash.Api.Models;
using QuickCash.Api.Repositories.Interfaces;
using QuickCash.Api.Services.Interfaces;

namespace QuickCash.Api.Services;

public class OverviewService : IOverviewService
{
    private readonly ITransactionRepository _transactions;

    public OverviewService(ITransactionRepository transactions)
    {
        _transactions = transactions;
    }

    public async Task<MonthlyOverviewDto> GetMonthlyAsync(int year, int month)
    {
        if (year < 2000 || year > 2100)
            throw new ArgumentException("Year is out of range.");

        if (month < 1 || month > 12)
            throw new ArgumentException("Month must be 1-12.");

        var entities = await _transactions.GetByMonthAsync(year, month);

        var dtos = entities.Select(t =>
        {
            if (t.Category == null)
                throw new InvalidOperationException("Transaction Category was not loaded.");

            return new TransactionDto
            {
                Id = t.Id,
                Date = t.Date,
                Amount = t.Amount,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                CategoryType = t.Category.Type,
                Note = t.Note
            };
        }).ToList();

        var totalIncome = dtos
            .Where(t => t.CategoryType == CategoryType.Income)
            .Sum(t => t.Amount);

        var totalExpenses = dtos
            .Where(t => t.CategoryType == CategoryType.Expense)
            .Sum(t => t.Amount);

        return new MonthlyOverviewDto
        {
            Year = year,
            Month = month,
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            Net = totalIncome - totalExpenses,
            Transactions = dtos
        };
    }
}
