using FinanceManagement.Models.Authorization;

namespace FinanceManagement.Blazor.Server.Services.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResult> LoginAsync(string email, string password);
        Task<RegistrationResult> RegisterAsync(string name, string email, string password);
    }
}
