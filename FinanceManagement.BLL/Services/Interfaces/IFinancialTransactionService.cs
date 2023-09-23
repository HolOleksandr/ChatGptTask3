using FinanceManagement.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.BLL.Services.Interfaces
{
    public interface IFinancialTransactionService
    {
        Task<IEnumerable<FinancialTransactionDTO>> GetAllTransactionsAsync();
        Task<FinancialTransactionDTO> GetTransactionByIdAsync(int id);
        Task<FinancialTransactionDTO> CreateTransactionAsync(FinancialTransactionDTO transactionDto);
        Task UpdateTransactionAsync(FinancialTransactionDTO transactionDto);
        Task DeleteTransactionAsync(int id);
    }
}
