using QuickCash.Api.Dtos.Transactions;

namespace QuickCash.Api.Dtos.Overview;

public class MonthlyOverviewDto
{
    public int Year { get; set; }
    public int Month { get; set; }

    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal Net { get; set; }

    public List<TransactionDto> Transactions { get; set; } = new();
}
