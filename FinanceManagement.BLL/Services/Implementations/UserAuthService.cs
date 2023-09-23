using AutoMapper;
using FinanceManagement.BLL.Exceptions;
using FinanceManagement.BLL.Services.Interfaces;
using FinanceManagement.Core;
using FinanceManagement.Core.Enums;
using FinanceManagement.Models.Authorization;
using FinanceManagement.Models.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinanceManagement.BLL.Services.Implementations
{
    public class UserAuthService : IUserAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWTSettings _jwtSettings;
        private ApplicationUser? _user;

        public UserAuthService(UserManager<ApplicationUser> userManager, IOptions<JWTSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> CreateTokenAsync()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationModel userModel)
        {
            CheckPasswordInModel(userModel);
            var user = new ApplicationUser
            {
                Email = userModel.Email,
                UserName = userModel.Email,
                Name = userModel.Name,
                PhoneNumber = userModel.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (result.Succeeded)
            {
                await SetupDefaultUserRole(user);
            }

            return result;
        }

        private static void CheckPasswordInModel(UserRegistrationModel userModel)
        {
            if (userModel.Password == null)
                throw new FinanceManagementException("Password is empty");
        }

        private async Task SetupDefaultUserRole(ApplicationUser user)
        {
            var defaultRole = ApplicationRoles.User.ToString();
            await _userManager.AddToRoleAsync(user, defaultRole);
        }

        public async Task<bool> ValidateUserAsync(UserLoginModel userModel)
        {
            _user = await _userManager.FindByNameAsync(userModel.Email);
            return _user != null && await _userManager.CheckPasswordAsync(_user, userModel.Password);
        }
                
        private SigningCredentials GetSigningCredentials()
        {
            var key = _jwtSettings.Secret;
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user!.Email),
                new Claim(ClaimTypes.NameIdentifier, _user.Id)
            };
            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationTimeInMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials);
            return token;
        }
    }
}
