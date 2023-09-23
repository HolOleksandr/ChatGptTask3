using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Models.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int BudgetId { get; set; }
        public BudgetDTO Budget { get; set; } = null!;
        public ICollection<ExpenseDTO> Expenses { get; set; } = null!;
    }
}
