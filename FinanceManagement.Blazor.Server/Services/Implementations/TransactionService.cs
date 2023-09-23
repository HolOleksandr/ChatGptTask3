using FinanceManagement.Blazor.Server.Models;
using FinanceManagement.Blazor.Server.Services.Interfaces;
using FinanceManagement.Models.DTOs;
using Microsoft.Extensions.Options;

namespace FinanceManagement.Blazor.Server.Services.Implementations
{
    public class TransactionsService : ITransactionsService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;

        public TransactionsService(HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _httpClient = httpClient;
            _appSettings = appSettings.Value;
        }

        private static async Task<T> CheckAndDeserializeResponse<T>(HttpResponseMessage response, string errorMessage)
        {
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<T>();
            return result == null ? throw new InvalidOperationException(errorMessage) : result;
        }

        public async Task<IList<FinancialTransactionDTO>> GetAllTransactionsAsync()
        {
            var response = await _httpClient.GetAsync($"{_appSettings.ApiBaseUrl}/Transaction");
            return await CheckAndDeserializeResponse<IList<FinancialTransactionDTO>>(response, "Error retrieving Transactions.");
        }

        public async Task<FinancialTransactionDTO> GetTransactionByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_appSettings.ApiBaseUrl}/Transaction/{id}");
            return await CheckAndDeserializeResponse<FinancialTransactionDTO>(response, $"Error retrieving Transaction with ID: {id}");
        }

        public async Task<FinancialTransactionDTO> CreateTransactionAsync(FinancialTransactionDTO transactionDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_appSettings.ApiBaseUrl}/Transaction", transactionDto);
            return await CheckAndDeserializeResponse<FinancialTransactionDTO>(response, "Error creating Transaction.");
        }

        public async Task UpdateTransactionAsync(FinancialTransactionDTO transactionDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_appSettings.ApiBaseUrl}/Transaction/{transactionDto.Id}", transactionDto);
            await CheckAndDeserializeResponse<object>(response, $"Error updating Transaction with ID: {transactionDto.Id}");
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_appSettings.ApiBaseUrl}/Transaction/{id}");
            await CheckAndDeserializeResponse<object>(response, $"Error deleting Transaction with ID: {id}");
        }
    }
}
