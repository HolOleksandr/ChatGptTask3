using FinanceManagement.Blazor.Server.Models;
using FinanceManagement.Blazor.Server.Services.Interfaces;
using FinanceManagement.Models.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Options;

namespace FinanceManagement.Blazor.Server.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;
        private readonly ProtectedLocalStorage _protectedLocalStorage;

        public AccountService(HttpClient httpClient, IOptions<AppSettings> appSettings, ProtectedLocalStorage protectedLocalStorage)
        {
            _httpClient = httpClient;
            _appSettings = appSettings.Value;
            _protectedLocalStorage = protectedLocalStorage;
        }

        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            UserLoginModel userLoginModel = new() { Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync($"{_appSettings.ApiBaseUrl}/Account/login", userLoginModel);
            response.EnsureSuccessStatusCode();
            LoginResult loginResult = (await response.Content.ReadFromJsonAsync<LoginResult>())
                              ?? new LoginResult { Success = false, Message = "Unknown error occurred during login." };
            await SaveTokenIfSuccessed(loginResult);
            return loginResult;
        }

        private async Task SaveTokenIfSuccessed(LoginResult loginResult)
        {
            if (loginResult.Success && !string.IsNullOrEmpty(loginResult.Token))
            {
                await _protectedLocalStorage.SetAsync("accessToken", loginResult.Token);
            }
        }

        public async Task<RegistrationResult> RegisterAsync(string name, string email, string password)
        {
            UserRegistrationModel userRegistrationModel = new() { Name = name, Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync($"{_appSettings.ApiBaseUrl}/Account/register", userRegistrationModel);
            response.EnsureSuccessStatusCode();
            RegistrationResult registrationResult = (await response.Content.ReadFromJsonAsync<RegistrationResult>())
                                           ?? new RegistrationResult { Success = false, Errors = new[] { "Unknown error occurred during registration." } };
            return registrationResult;
        }
    }
}
