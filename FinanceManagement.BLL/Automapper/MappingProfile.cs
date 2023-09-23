using AutoMapper;
using FinanceManagement.Core;
using FinanceManagement.Models.Authorization;
using FinanceManagement.Models.DTOs;

namespace FinanceManagement.BLL.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Budget, BudgetDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Expense, ExpenseDTO>().ReverseMap();
            CreateMap<FinancialTransaction, FinancialTransactionDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserRegistrationModel>().ReverseMap();
        }
    }
}
