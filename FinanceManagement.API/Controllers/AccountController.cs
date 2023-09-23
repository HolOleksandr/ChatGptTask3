using FinanceManagement.BLL.Services.Implementations;
using FinanceManagement.BLL.Services.Interfaces;
using FinanceManagement.Models.Authorization;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IValidator<UserLoginModel> _userLoginValidator;
        private readonly IValidator<UserRegistrationModel> _userRegistrationValidator;
        private readonly IUserAuthService _userAuthService;

        public AccountController(IUserAuthService userAuthService,
            IValidator<UserLoginModel> userLoginValidator,
            IValidator<UserRegistrationModel> userRegistrationValidator)
        {
            _userAuthService = userAuthService;
            _userLoginValidator = userLoginValidator;
            _userRegistrationValidator = userRegistrationValidator;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser(UserRegistrationModel userModel)
        {
            var validation = await _userRegistrationValidator.ValidateAsync(userModel);
            if (IsInvalid(validation))
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var authResult = await _userAuthService.RegisterUserAsync(userModel);
            if (DidNotSucceed(authResult))
            {
                var errors = authResult.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResult { Success = false, Errors = errors });
            }
            return Accepted(new RegistrationResult { Success = true });
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Authenticate(UserLoginModel user)
        {
            var modelValidation = await _userLoginValidator.ValidateAsync(user);
            if (IsInvalid(modelValidation))
                return BadRequest(modelValidation.Errors.Select(e => e.ErrorMessage));

            if (await UserNotValidAsync(user))
            {
                return Unauthorized(new LoginResult()
                {
                    Success = false,
                    Message = "Invalid Email or Password."
                });
            }
            var token = await _userAuthService.CreateTokenAsync();
            return Accepted(new LoginResult()
            {
                Success = true,
                Message = "Login successful",
                Token = token
            });
        }

        private static bool IsInvalid(FluentValidation.Results.ValidationResult validationResult)
        {
            return !validationResult.IsValid;
        }
        private static bool DidNotSucceed(IdentityResult result)
        {
            return !result.Succeeded;
        }

        private async Task<bool> UserNotValidAsync(UserLoginModel user)
        {
            return !await _userAuthService.ValidateUserAsync(user);
        }
    }
}