using FinanceManagement.Core;
using FinanceManagement.DAL.Data;
using FinanceManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.DAL.Repositories.Implementations
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(FinanceDbContext context) : base(context)
        {
        }
    }
}
