using FinanceManagement.Core;
using FinanceManagement.DAL.Data;
using FinanceManagement.DAL.Repositories.Interfaces;

namespace FinanceManagement.DAL.Repositories.Implementations
{
    public class PlaidTransactionRepository : Repository<PlaidTransaction>, IPlaidTransactionRepository
    {
        public PlaidTransactionRepository(FinanceDbContext context) : base(context)
        {
        }
    }
}
