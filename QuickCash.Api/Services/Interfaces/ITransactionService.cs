using QuickCash.Api.Dtos.Transactions;

namespace QuickCash.Api.Services.Interfaces;

public interface ITransactionService
{
    Task<List<TransactionDto>> GetByMonthAsync(int year, int month);
    Task<TransactionDto> CreateAsync(CreateTransactionRequest request);
    Task DeleteAsync(int id);
}
