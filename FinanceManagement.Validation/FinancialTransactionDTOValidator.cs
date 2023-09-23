using FinanceManagement.Models.DTOs;
using FluentValidation;

namespace FinanceManagement.Validation
{
    public class FinancialTransactionDTOValidator : AbstractValidator<FinancialTransactionDTO>
    {
        public FinancialTransactionDTOValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage("ID must be greater than or equal to 0");
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500).WithMessage("Description must be a non-empty string and have a maximum length of 500 characters");
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than or equal to 0");
            RuleFor(x => x.Date).NotNull().WithMessage("Date must not be null");
        }
    }
}
