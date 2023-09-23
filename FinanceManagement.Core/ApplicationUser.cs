using Microsoft.AspNetCore.Identity;
using System.Transactions;

namespace FinanceManagement.Core
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<FinancialTransaction> Transactions { get; set; } = new List<FinancialTransaction>();
    }
}
