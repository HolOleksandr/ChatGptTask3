using FinanceManagement.Models.DTOs;
using FluentValidation;

namespace FinanceManagement.Validation
{
    public class CategoryDTOValidator : AbstractValidator<CategoryDTO>
    {
        public CategoryDTOValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage("ID must be greater than or equal to 0");
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200).WithMessage("Name must be a non-empty string and have a maximum length of 200 characters");
        }
    }
}
