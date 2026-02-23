using QuickCash.Api.Dtos.Overview;

namespace QuickCash.Api.Services.Interfaces;

public interface IOverviewService
{
    Task<MonthlyOverviewDto> GetMonthlyAsync(int year, int month);
}
