using System.Net.Http.Json;
using QuickCash.Ui.Api.Models;

namespace QuickCash.Ui.Api.Services;

public class TransactionsApi
{
    private readonly HttpClient _http;

    public TransactionsApi(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<TransactionDto>> GetByMonthAsync(int year, int month)
    {
        var url = $"api/transactions?year={year}&month={month}";
        var result = await _http.GetFromJsonAsync<List<TransactionDto>>(url);
        return result ?? new List<TransactionDto>();
    }

    public async Task<TransactionDto> CreateAsync(CreateTransactionRequest request)
    {
        var resp = await _http.PostAsJsonAsync("api/transactions", request);

        // If API returns 400, surface the message
        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync();
            throw new InvalidOperationException(body);
        }

        var created = await resp.Content.ReadFromJsonAsync<TransactionDto>();
        if (created == null) throw new InvalidOperationException("API returned no transaction.");
        return created;
    }

    public async Task DeleteAsync(int id)
    {
        var resp = await _http.DeleteAsync($"api/transactions/{id}");
        if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
            return; // optional: treat missing as no-op in UI

        resp.EnsureSuccessStatusCode();
    }
}
