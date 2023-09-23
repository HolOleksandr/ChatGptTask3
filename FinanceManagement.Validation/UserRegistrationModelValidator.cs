using FinanceManagement.Models.Authorization;
using FluentValidation;

namespace FinanceManagement.Validation
{
    public class UserRegistrationModelValidator : AbstractValidator<UserRegistrationModel>
    {
        public UserRegistrationModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not in the correct format");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm Password is required")
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.Name).NotEmpty().WithMessage("First Name is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone Number is required")
                .Matches(@"^(\+[0-9]{1,3}[0-9]{1,14})$").WithMessage("Invalid phone number format");

            RuleFor(x => x.BirthDate)
                .Must(BeMinimumAge).WithMessage("Age must be at least 18 years")
                .Must(BeMaximumAge).WithMessage("Age must be less than 100 years");
        }

        private bool BeMinimumAge(DateTime? birthDate)
        {
            if (!birthDate.HasValue) return true;

            int minimumAge = 18;
            DateTime currentDate = DateTime.UtcNow;
            int age = CalculateAge(currentDate, birthDate.Value);

            return age >= minimumAge;
        }

        private bool BeMaximumAge(DateTime? birthDate)
        {
            if (!birthDate.HasValue) return true;

            int maximumAge = 100;
            DateTime currentDate = DateTime.UtcNow;
            int age = CalculateAge(currentDate, birthDate.Value);

            return age <= maximumAge;
        }

        private static int CalculateAge(DateTime currentDate, DateTime birthDate)
        {
            var age = currentDate.Year - birthDate.Year;

            if (birthDate > currentDate.AddYears(-age)) age--;

            return age;
        }
    }
}
