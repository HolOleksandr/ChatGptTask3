using FinanceManagement.Core;
using FinanceManagement.DAL.Data;
using FinanceManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FinanceManagement.DAL.Repositories.Implementations
{
    public class TransactionRepository : Repository<FinancialTransaction>, ITransactionRepository
    {
        public TransactionRepository(FinanceDbContext context) : base(context)
        {
        }
    }
}
