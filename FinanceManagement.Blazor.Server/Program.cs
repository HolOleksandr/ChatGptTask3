using FinanceManagement.Blazor.Server.Data;
using FinanceManagement.Blazor.Server.Models;
using FinanceManagement.Blazor.Server.Services.Implementations;
using FinanceManagement.Blazor.Server.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using ProtectedLocalStore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionsService, TransactionsService>();
builder.Configuration.Bind("AppSettings", builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings")));
builder.Services.AddProtectedLocalStore(new EncryptionService());

builder.Services
    .AddSingleton(sp => new HttpClient() { BaseAddress = new Uri(builder.Configuration.GetSection("ApiBaseUrl").ToString()!)});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
