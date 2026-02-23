using System.Net.Http.Json;
using QuickCash.Ui.Api.Models;

namespace QuickCash.Ui.Api.Services;

public class CategoriesApi
{
    private readonly HttpClient _http;

    public CategoriesApi(HttpClient http) => _http = http;

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<List<CategoryDto>>("api/categories");
        return result ?? new List<CategoryDto>();
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request)
    {
        var resp = await _http.PostAsJsonAsync("api/categories", request);
        if (!resp.IsSuccessStatusCode)
            throw new Exception(await resp.Content.ReadAsStringAsync());

        return (await resp.Content.ReadFromJsonAsync<CategoryDto>())!;
    }

    public async Task DeleteAsync(int id)
    {
        var resp = await _http.DeleteAsync($"api/categories/{id}");

        if (resp.IsSuccessStatusCode)
            return;

        // This is where your 409 message (“Category is in use…”) will surface cleanly
        throw new Exception(await resp.Content.ReadAsStringAsync());
    }
}
