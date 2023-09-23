using AutoMapper;
using FinanceManagement.BLL.Exceptions;
using FinanceManagement.BLL.Services.Interfaces;
using FinanceManagement.Core;
using FinanceManagement.DAL.Repositories.Interfaces;
using FinanceManagement.DAL.UnitOfWork.Interfaces;
using FinanceManagement.Models.DTOs;

namespace FinanceManagement.BLL.Services.Implementations
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExpenseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExpenseDTO>> GetAllExpensesAsync()
        {
            var expenses = await _unitOfWork.GetRepository<IExpenseRepository>().GetAllAsync();
            return _mapper.Map<IEnumerable<ExpenseDTO>>(expenses);
        }

        public async Task<ExpenseDTO> GetExpenseByIdAsync(int id)
        {
            var expense = await GetExpenseOrThrowAsync(id);
            return _mapper.Map<ExpenseDTO>(expense);
        }

        public async Task<ExpenseDTO> CreateExpenseAsync(ExpenseDTO expenseDto)
        {
            var expense = _mapper.Map<Expense>(expenseDto);
            await _unitOfWork.GetRepository<IExpenseRepository>().AddAsync(expense);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ExpenseDTO>(expense);
        }

        public async Task UpdateExpenseAsync(ExpenseDTO expenseDto)
        {
            var expense = _mapper.Map<Expense>(expenseDto);
            _unitOfWork.GetRepository<IExpenseRepository>().Update(expense);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteExpenseAsync(int id)
        {
            var expense = await GetExpenseOrThrowAsync(id);
            await _unitOfWork.GetRepository<IExpenseRepository>().RemoveAsync(expense.Id);
            await _unitOfWork.SaveAsync();
        }

        private async Task<Expense> GetExpenseOrThrowAsync(int id)
        {
            var expense = await _unitOfWork.GetRepository<IExpenseRepository>().GetByIdAsync(id);

            return expense ?? throw new FinanceManagementException($"Expense with ID {id} not found");
        }
    }
}
