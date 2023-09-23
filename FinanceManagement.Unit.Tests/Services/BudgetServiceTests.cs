using AutoMapper;
using FinanceManagement.BLL.Automapper;
using FinanceManagement.BLL.Exceptions;
using FinanceManagement.BLL.Services.Implementations;
using FinanceManagement.Core;
using FinanceManagement.DAL.Repositories.Interfaces;
using FinanceManagement.DAL.UnitOfWork.Interfaces;
using FinanceManagement.Models.DTOs;
using Moq;

namespace FinanceManagement.Unit.Tests.Services
{
    public class BudgetServiceTests
    {
        private BudgetService _budgetService;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IBudgetRepository> _budgetRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _budgetRepositoryMock = new Mock<IBudgetRepository>();
            _unitOfWorkMock.Setup(uow => uow.GetRepository<IBudgetRepository>()).Returns(_budgetRepositoryMock.Object);

            _budgetService = new BudgetService(_unitOfWorkMock.Object, mapper);
        }

        [Test]
        public async Task GetAllBudgetsAsync_ShouldReturnAllBudgets()
        {
            // Arrange
            var budgets = new List<Budget>
            {
                new Budget { Id = 1, UserId = "1", Name = "Budget A", Amount = 500 },
                new Budget { Id = 2, UserId = "1", Name = "Budget B", Amount = 600 },
            };
            _budgetRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(budgets);

            // Act
            var result = await _budgetService.GetAllBudgetsAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<IEnumerable<BudgetDTO>>());
                Assert.That(result.Count(), Is.EqualTo(2));
            });
        }

        [Test]
        public async Task GetBudgetByIdAsync_ShouldReturnBudgetWithGivenId()
        {
            // Arrange
            int testBudgetId = 1;
            var expectedBudget = new Budget { Id = testBudgetId, UserId = "1", Name = "Budget A", Amount = 500 };
            _budgetRepositoryMock.Setup(repo => repo.GetByIdAsync(testBudgetId)).ReturnsAsync(expectedBudget);

            // Act
            var result = await _budgetService.GetBudgetByIdAsync(testBudgetId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(testBudgetId));
            });
        }

        [Test]
        public async Task CreateBudgetAsync_ShouldCreateBudgetSuccessfully()
        {
            // Arrange
            var budgetDto = new BudgetDTO { UserId = "1", Name = "Budget A", Amount = 500 };

            // Act
            var result = await _budgetService.CreateBudgetAsync(budgetDto);

            // Assert
            _budgetRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Budget>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateBudgetAsync_ShouldUpdateBudgetSuccessfully()
        {
            // Arrange
            var budgetDto = new BudgetDTO { Id = 1, UserId = "1", Name = "Updated Budget A", Amount = 600 };
            var existingBudget = new Budget { Id = 1, UserId = "1", Name = "Budget A", Amount = 500 };
            _budgetRepositoryMock.Setup(repo => repo.GetByIdAsync(budgetDto.Id)).ReturnsAsync(existingBudget);

            // Act
            await _budgetService.UpdateBudgetAsync(budgetDto);

            // Assert
            _budgetRepositoryMock.Verify(repo => repo.Update(It.Is<Budget>(b => b.Name == budgetDto.Name)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteBudgetAsync_ShouldRemoveBudgetSuccessfully()
        {
            // Arrange
            int testBudgetId = 1;
            var existingBudget = new Budget { Id = testBudgetId, UserId = "1", Name = "Budget A", Amount = 500 };
            _budgetRepositoryMock.Setup(repo => repo.GetByIdAsync(testBudgetId)).ReturnsAsync(existingBudget);

            // Act
            await _budgetService.DeleteBudgetAsync(testBudgetId);

            // Assert
            _budgetRepositoryMock.Verify(repo => repo.RemoveAsync(testBudgetId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Test]
        public void GetBudgetByIdAsync_ShouldThrowNotFoundException_WhenBudgetNotFound()
        {
            // Arrange
            int testBudgetId = 1;
            _budgetRepositoryMock.Setup(repo => repo.GetByIdAsync(testBudgetId)).Returns(Task.FromResult<Budget?>(null));

            // Act & Assert
            Assert.ThrowsAsync<FinanceManagementException>(() => _budgetService.GetBudgetByIdAsync(testBudgetId));
        }
    }
}
