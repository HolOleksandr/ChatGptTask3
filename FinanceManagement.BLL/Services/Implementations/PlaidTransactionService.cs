using AutoMapper;
using FinanceManagement.BLL.Integrations;
using FinanceManagement.DAL.UnitOfWork.Interfaces;
using FinanceManagement.Models.DTOs;
using Going.Plaid.Entity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;

namespace FinanceManagement.BLL.Services.Implementations
{
    public class PlaidTransactionService
    {
        private readonly PlaidIntegration _plaidIntegration;
        
        public PlaidTransactionService(
            PlaidIntegration plaidIntegration
        )
        {
            _plaidIntegration = plaidIntegration;
            
        }

        public static Task<IList<FinancialTransactionDTO>> FetchTransactionsUsingAccessTokenAsync()
        {
            var financialTransactions = new List<FinancialTransactionDTO>();
            return Task.FromResult<IList<FinancialTransactionDTO>>(financialTransactions);
        }

        public async Task<string> ExchangePublicTokenAsync()
        {
            string accessToken = await _plaidIntegration.GetAccessTokenAsync();
            return accessToken;
        }
    }
}