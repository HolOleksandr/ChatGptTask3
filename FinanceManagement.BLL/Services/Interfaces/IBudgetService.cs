using FinanceManagement.Models.DTOs;

namespace FinanceManagement.BLL.Services.Interfaces
{
    public interface IBudgetService
    {
        Task<IEnumerable<BudgetDTO>> GetAllBudgetsAsync();
        Task<BudgetDTO> GetBudgetByIdAsync(int id);
        Task<BudgetDTO> CreateBudgetAsync(BudgetDTO budgetDto);
        Task UpdateBudgetAsync(BudgetDTO budgetDto);
        Task DeleteBudgetAsync(int id);
    }
}
