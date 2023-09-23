using AutoMapper;
using FinanceManagement.BLL.Exceptions;
using FinanceManagement.BLL.Services.Interfaces;
using FinanceManagement.Core;
using FinanceManagement.DAL.Repositories.Interfaces;
using FinanceManagement.DAL.UnitOfWork.Interfaces;
using FinanceManagement.Models.DTOs;

namespace FinanceManagement.BLL.Services.Implementations
{
    public class BudgetService : IBudgetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BudgetService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BudgetDTO>> GetAllBudgetsAsync()
        {
            var budgets = await _unitOfWork.GetRepository<IBudgetRepository>().GetAllAsync();
            return _mapper.Map<IEnumerable<BudgetDTO>>(budgets);
        }

        public async Task<BudgetDTO> GetBudgetByIdAsync(int id)
        {
            var budget = await GetBudgetOrThrowAsync(id);

            return _mapper.Map<BudgetDTO>(budget);
        }

        public async Task<BudgetDTO> CreateBudgetAsync(BudgetDTO budgetDto)
        {
            var budget = _mapper.Map<Budget>(budgetDto);
            await _unitOfWork.GetRepository<IBudgetRepository>().AddAsync(budget);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<BudgetDTO>(budget);
        }

        public async Task UpdateBudgetAsync(BudgetDTO budgetDto)
        {
            var budget = _mapper.Map<Budget>(budgetDto);
            _unitOfWork.GetRepository<IBudgetRepository>().Update(budget);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteBudgetAsync(int id)
        {
            var budget = await GetBudgetOrThrowAsync(id);

            await _unitOfWork.GetRepository<IBudgetRepository>().RemoveAsync(budget.Id);
            await _unitOfWork.SaveAsync();
        }
        private async Task<Budget> GetBudgetOrThrowAsync(int id)
        {
            var budget = await _unitOfWork.GetRepository<IBudgetRepository>().GetByIdAsync(id);

            return budget ?? throw new FinanceManagementException($"Budget with ID {id} not found");
        }
    }
}
