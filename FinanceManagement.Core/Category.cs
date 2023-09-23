using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int BudgetId { get; set; }
        public Budget Budget { get; set; } = null!;
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
