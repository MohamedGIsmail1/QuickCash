using Microsoft.EntityFrameworkCore;
using QuickCash.Api.Data;
using QuickCash.Api.Models;
using QuickCash.Api.Repositories.Interfaces;

namespace QuickCash.Api.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly QuickCashDbContext _db;

    public TransactionRepository(QuickCashDbContext db)
    {
        _db = db;
    }

    public async Task<List<Transaction>> GetByMonthAsync(int year, int month)
    {
        return await _db.Transactions
            .Include(t => t.Category) // needed because overview needs category type/name
            .Where(t => t.Date.Year == year && t.Date.Month == month)
            .OrderByDescending(t => t.Date)
            .ThenByDescending(t => t.Id)
            .ToListAsync();
    }

    public async Task<Transaction?> GetByIdAsync(int id)
    {
        return await _db.Transactions
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _db.Transactions.AddAsync(transaction);
    }

    public Task DeleteAsync(Transaction transaction)
    {
        _db.Transactions.Remove(transaction);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
