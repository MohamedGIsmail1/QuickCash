using System.Net.Http.Json;
using QuickCash.Ui.Api.Models;

namespace QuickCash.Ui.Api.Services;

public class OverviewApi
{
    private readonly HttpClient _http;

    public OverviewApi(HttpClient http)
    {
        _http = http;
    }

    public async Task<OverviewDto> GetAsync(int year, int month)
    {
        var url = $"api/overview?year={year}&month={month}";
        var res = await _http.GetAsync(url);

        if (res.IsSuccessStatusCode)
        {
            var dto = await res.Content.ReadFromJsonAsync<OverviewDto>();
            if (dto == null) throw new Exception("Empty response from overview endpoint.");
            return dto;
        }

        // match your existing pattern: show API error if it returns { error = "..." }
        var body = await res.Content.ReadAsStringAsync();
        throw new Exception(body);
    }
}
