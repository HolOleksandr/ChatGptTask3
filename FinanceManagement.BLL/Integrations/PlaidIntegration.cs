using Going.Plaid;
using Going.Plaid.Entity;
using Microsoft.Extensions.Configuration;
using Going.Plaid.Link;
using Going.Plaid.Transactions;

namespace FinanceManagement.BLL.Integrations
{
    public class PlaidIntegration
    {
        private readonly PlaidClient _plaidClient;
        private readonly IConfiguration _configuration;

        public PlaidIntegration(PlaidClient plaidClient, IConfiguration configuration)
        {
            _plaidClient = plaidClient;
            _configuration = configuration;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var response = await _plaidClient.LinkTokenCreateAsync(new LinkTokenCreateRequest()
            {
                
            });
            return response.LinkToken;
        }

        public async Task<IList<Transaction>> GetTransactionsAsync(string accessToken)
        {
            var response = await _plaidClient.TransactionsGetAsync(new TransactionsGetRequest()
            {
                AccessToken = accessToken,
                ClientId = _configuration["Plaid:ClientId"],
                Secret = _configuration["Plaid:Secret"],
            });
            return (IList<Transaction>)response.Transactions;
        }
    }
}
