using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using QuickCash.Ui;
using QuickCash.Ui.Api.Services;
using System.Net.Http.Json;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var configClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var config = await configClient.GetFromJsonAsync<Dictionary<string, string>>("appsettings.json");

if (config is null || !config.TryGetValue("ApiBaseUrl", out var apiBaseUrl) || string.IsNullOrWhiteSpace(apiBaseUrl))
    throw new InvalidOperationException("Missing ApiBaseUrl in QuickCash.Ui/wwwroot/appsettings.json");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
builder.Services.AddScoped<QuickCash.Ui.Api.Services.TransactionsApi>();
builder.Services.AddScoped<QuickCash.Ui.Api.Services.CategoriesApi>();
builder.Services.AddScoped<QuickCash.Ui.Api.Services.OverviewApi>();


await builder.Build().RunAsync();
