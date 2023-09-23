using FinanceManagement.Models.DTOs;
using FluentValidation;

namespace FinanceManagement.Validation
{
    public class BudgetDTOValidator : AbstractValidator<BudgetDTO>
    {
        public BudgetDTOValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage("ID must be greater than or equal to 0");
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200).WithMessage("Name must be a non-empty string and have a maximum length of 200 characters");
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than or equal to 0");
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate).WithMessage("StartDate must be less than EndDate");
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).WithMessage("EndDate must be greater than StartDate");
        }
    }
}
