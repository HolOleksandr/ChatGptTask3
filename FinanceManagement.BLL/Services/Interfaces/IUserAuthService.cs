using FinanceManagement.Models.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FinanceManagement.BLL.Services.Interfaces
{
    public interface IUserAuthService
    {
        Task<string> CreateTokenAsync();
        Task<IdentityResult> RegisterUserAsync(UserRegistrationModel userModel);
        Task<bool> ValidateUserAsync(UserLoginModel userModel);
    }
}
