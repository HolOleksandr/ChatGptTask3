using AutoMapper;
using FinanceManagement.BLL.Exceptions;
using FinanceManagement.BLL.Services.Interfaces;
using FinanceManagement.Core;
using FinanceManagement.DAL.Repositories.Interfaces;
using FinanceManagement.DAL.UnitOfWork.Interfaces;
using FinanceManagement.Models.DTOs;

namespace FinanceManagement.BLL.Services.Implementations
{
    public class FinancialTransactionService : IFinancialTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FinancialTransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FinancialTransactionDTO>> GetAllTransactionsAsync()
        {
            var transactions = await _unitOfWork.GetRepository<ITransactionRepository>().GetAllAsync();
            return _mapper.Map<IEnumerable<FinancialTransactionDTO>>(transactions);
        }

        public async Task<FinancialTransactionDTO> GetTransactionByIdAsync(int id)
        {
            var transaction = await GetTransactionOrThrowAsync(id);
            return _mapper.Map<FinancialTransactionDTO>(transaction);
        }

        public async Task<FinancialTransactionDTO> CreateTransactionAsync(FinancialTransactionDTO transactionDto)
        {
            var transaction = _mapper.Map<FinancialTransaction>(transactionDto);
            await _unitOfWork.GetRepository<ITransactionRepository>().AddAsync(transaction);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<FinancialTransactionDTO>(transaction);
        }

        public async Task UpdateTransactionAsync(FinancialTransactionDTO transactionDto)
        {
            var transaction = _mapper.Map<FinancialTransaction>(transactionDto);
            _unitOfWork.GetRepository<ITransactionRepository>().Update(transaction);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await GetTransactionOrThrowAsync(id);
            await _unitOfWork.GetRepository<ITransactionRepository>().RemoveAsync(transaction.Id);
            await _unitOfWork.SaveAsync();
        }

        private async Task<FinancialTransaction> GetTransactionOrThrowAsync(int id)
        {
            var transaction = await _unitOfWork.GetRepository<ITransactionRepository>().GetByIdAsync(id);

            return transaction ?? throw new FinanceManagementException($"Financial transaction with ID {id} not found");
        }
    }
}
