using FinanceManagement.Models.DTOs;

namespace FinanceManagement.Blazor.Server.Services.Interfaces
{
    public interface ITransactionsService
    {
        Task<IList<FinancialTransactionDTO>> GetAllTransactionsAsync();
        Task<FinancialTransactionDTO> GetTransactionByIdAsync(int id);
        Task<FinancialTransactionDTO> CreateTransactionAsync(FinancialTransactionDTO transactionDto);
        Task UpdateTransactionAsync(FinancialTransactionDTO transactionDto);
        Task DeleteTransactionAsync(int id);
    }
}
