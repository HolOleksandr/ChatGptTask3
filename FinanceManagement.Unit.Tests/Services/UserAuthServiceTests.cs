using AutoMapper;
using FinanceManagement.BLL.Automapper;
using FinanceManagement.BLL.Services.Implementations;
using FinanceManagement.Core;
using FinanceManagement.Models.Authorization;
using FinanceManagement.Models.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Unit.Tests.Services
{
    public class UserAuthServiceTests
    {
        private UserAuthService _userAuthService;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<IOptions<JWTSettings>> _jwtSettingsMock;
        private readonly IMapper _mapper = null!;

        [SetUp]
        public void SetUp()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _jwtSettingsMock = new Mock<IOptions<JWTSettings>>();

            _jwtSettingsMock.Setup(options => options.Value)
                .Returns(new JWTSettings
                {
                    Secret = "test-secret-with-more-than-16-chars",
                    ExpirationTimeInMinutes = 60
                });

            _userAuthService = new UserAuthService(_userManagerMock.Object, _jwtSettingsMock.Object);
        }

        [Test]
        public async Task RegisterUserAsync_ShouldRegisterUserSuccessfully()
        {
            // Arrange
            var userRegistrationModel = new UserRegistrationModel 
            { 
                Name = "FirstName",
                LastName = "LastName",
                PhoneNumber = "1234567890",
                Email = "test@example.com", 
                Password = "Test1234$",
                BirthDate = DateTime.Now,
                ConfirmPassword = "Test1234$" 
            };
            var identityResult = IdentityResult.Success;

            _userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(identityResult);
            _userManagerMock.Setup(userManager => userManager.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(identityResult);

            // Act
            var result = await _userAuthService.RegisterUserAsync(userRegistrationModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(identityResult));
                _userManagerMock.Verify(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
                _userManagerMock.Verify(userManager => userManager.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            });
        }

        [Test]
        public async Task ValidateUserAsync_ShouldValidateUserSuccessfully()
        {
            // Arrange
            var userLoginModel = new UserLoginModel { Email = "test@example.com", Password = "Test1234$" };
            var applicationUser = new ApplicationUser { Email = userLoginModel.Email, UserName = userLoginModel.Email };

            _userManagerMock.Setup(userManager => userManager.FindByNameAsync(userLoginModel.Email))
                .ReturnsAsync(applicationUser);
            _userManagerMock.Setup(userManager => userManager.CheckPasswordAsync(applicationUser, userLoginModel.Password))
                .ReturnsAsync(true);

            // Act
            var result = await _userAuthService.ValidateUserAsync(userLoginModel);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
