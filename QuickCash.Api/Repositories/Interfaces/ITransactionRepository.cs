using QuickCash.Api.Models;

namespace QuickCash.Api.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetByMonthAsync(int year, int month);
    Task<Transaction?> GetByIdAsync(int id);

    Task AddAsync(Transaction transaction);
    Task DeleteAsync(Transaction transaction);
    Task SaveChangesAsync();
}
