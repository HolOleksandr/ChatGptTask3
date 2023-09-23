using FinanceManagement.Models.DTOs;

namespace FinanceManagement.BLL.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<IEnumerable<ExpenseDTO>> GetAllExpensesAsync();
        Task<ExpenseDTO> GetExpenseByIdAsync(int id);
        Task<ExpenseDTO> CreateExpenseAsync(ExpenseDTO expenseDto);
        Task UpdateExpenseAsync(ExpenseDTO expenseDto);
        Task DeleteExpenseAsync(int id);
    }
}
