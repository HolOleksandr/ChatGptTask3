using FinanceManagement.Models.Authorization;
using FinanceManagement.Models.DTOs;

namespace FinanceManagement.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(string id);
        Task<UserDTO> CreateUserAsync(UserRegistrationModel userRegistrationModel);
        Task UpdateUserAsync(UserDTO userDto);
        Task DeleteUserAsync(string id);
    }
}
