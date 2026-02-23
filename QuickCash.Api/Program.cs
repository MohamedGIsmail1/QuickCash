using Microsoft.EntityFrameworkCore;
using QuickCash.Api.Data;
using QuickCash.Api.Repositories;
using QuickCash.Api.Repositories.Interfaces;
using QuickCash.Api.Services;
using QuickCash.Api.Services.Interfaces;
using QuickCash.Api.Services.MonthChanged;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Missing connection string: ConnectionStrings:Default");

builder.Services.AddDbContext<QuickCashDbContext>(options =>
    options.UseSqlite(connectionString));
    
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IOverviewService, OverviewService>();

builder.Services.AddScoped<IMonthChangedPublisher, NoOpMonthChangedPublisher>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("UiCors", policy =>
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin());
});


var app = builder.Build();

app.UseCors("UiCors");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// app.UseHttpsRedirection();
app.MapControllers();
app.Run();

