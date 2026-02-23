using QuickCash.Api.Dtos.Transactions;
using QuickCash.Api.Models;
using QuickCash.Api.Repositories.Interfaces;
using QuickCash.Api.Services.Interfaces;

namespace QuickCash.Api.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactions;
    private readonly ICategoryRepository _categories;
    private readonly IMonthChangedPublisher _publisher;

    public TransactionService(
        ITransactionRepository transactions,
        ICategoryRepository categories,
        IMonthChangedPublisher publisher)
    {
        _transactions = transactions;
        _categories = categories;
        _publisher = publisher;
    }

    public async Task<List<TransactionDto>> GetByMonthAsync(int year, int month)
    {
        ValidateYearMonth(year, month);

        var entities = await _transactions.GetByMonthAsync(year, month);

        return entities.Select(ToDto).ToList();
    }

    public async Task<TransactionDto> CreateAsync(CreateTransactionRequest request)
    {
        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be greater than 0.");

        if (request.Date == default)
            throw new ArgumentException("Date is required.");

        var category = await _categories.GetByIdAsync(request.CategoryId);
        if (category == null)
            throw new ArgumentException("Category must exist.");

        // Business rule: stored amounts are positive.
        var entity = new Transaction
        {
            Date = request.Date,
            Amount = Math.Abs(request.Amount),
            CategoryId = request.CategoryId,
            Note = string.IsNullOrWhiteSpace(request.Note) ? null : request.Note.Trim()
        };

        await _transactions.AddAsync(entity);
        await _transactions.SaveChangesAsync();

        // Notify month changed (future SignalR hook)
        await _publisher.PublishAsync(entity.Date.Year, entity.Date.Month);

        // Reload the created transaction with Category included (so DTO has category fields)
        var created = await _transactions.GetByIdAsync(entity.Id);
        if (created == null)
            throw new InvalidOperationException("Failed to load created transaction.");

        return ToDto(created);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _transactions.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("Transaction not found.");

        var year = entity.Date.Year;
        var month = entity.Date.Month;

        await _transactions.DeleteAsync(entity);
        await _transactions.SaveChangesAsync();

        await _publisher.PublishAsync(year, month);
    }

    private static TransactionDto ToDto(Transaction t)
    {
        // Category should be included by repository. If not, fail fast.
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
    }

    private static void ValidateYearMonth(int year, int month)
    {
        if (year < 2000 || year > 2100)
            throw new ArgumentException("Year is out of range.");

        if (month < 1 || month > 12)
            throw new ArgumentException("Month must be 1-12.");
    }
}
