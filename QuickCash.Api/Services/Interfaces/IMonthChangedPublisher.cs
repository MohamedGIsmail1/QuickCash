namespace QuickCash.Api.Services.Interfaces;

public interface IMonthChangedPublisher
{
    Task PublishAsync(int year, int month);
}
