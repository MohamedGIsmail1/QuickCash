using QuickCash.Api.Services.Interfaces;

namespace QuickCash.Api.Services.MonthChanged;

public class NoOpMonthChangedPublisher : IMonthChangedPublisher
{
    public Task PublishAsync(int year, int month)
    {
        return Task.CompletedTask;
    }
}
