using AutoMapper;
using FinanceManagement.BLL.Exceptions;
using FinanceManagement.BLL.Services.Interfaces;
using FinanceManagement.Core;
using FinanceManagement.Models.Authorization;
using FinanceManagement.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagement.BLL.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ThrowIfUserNotFound(user, id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> CreateUserAsync(UserRegistrationModel userRegistrationModel)
        {
            var user = _mapper.Map<ApplicationUser>(userRegistrationModel);
            var result = await _userManager.CreateAsync(user, userRegistrationModel.Password);
            ThrowIfIdentityResultFailed(result, "An error occurred during user creation.");
            return _mapper.Map<UserDTO>(user);
        }

        public async Task UpdateUserAsync(UserDTO userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);
            ThrowIfUserNotFound(user, userDto.Id);
            _mapper.Map(userDto, user);
            var result = await _userManager.UpdateAsync(user);
            ThrowIfIdentityResultFailed(result, "An error occurred during user update.");
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ThrowIfUserNotFound(user, id);
            var result = await _userManager.DeleteAsync(user);
            ThrowIfIdentityResultFailed(result, "An error occurred during user deletion.");
        }

        private static void ThrowIfUserNotFound(ApplicationUser user, string? id)
        {
            if (user == null)
            {
                throw new FinanceManagementException($"User with ID {id} not found");
            }
        }

        private static void ThrowIfIdentityResultFailed(IdentityResult result, string errorMessage)
        {
            if (!result.Succeeded)
            {
                throw new FinanceManagementException(errorMessage);
            }
        }
    }
}
