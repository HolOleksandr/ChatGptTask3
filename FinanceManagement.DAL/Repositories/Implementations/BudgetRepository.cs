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
    public class BudgetRepository : Repository<Budget>, IBudgetRepository
    {
        public BudgetRepository(FinanceDbContext context) : base(context)
        {
        }
    }
}
