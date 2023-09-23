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
    public class ExpenseServiceTests
    {
        private ExpenseService _expenseService;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IExpenseRepository> _expenseRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _expenseRepositoryMock = new Mock<IExpenseRepository>();
            _unitOfWorkMock.Setup(uow => uow.GetRepository<IExpenseRepository>()).Returns(_expenseRepositoryMock.Object);

            _expenseService = new ExpenseService(_unitOfWorkMock.Object, mapper);
        }

        [Test]
        public async Task GetAllExpensesAsync_ShouldReturnAllExpenses()
        {
            // Arrange
            var expenses = new List<Expense>
            {
                new Expense { Id = 1, Amount = 100, Description = "Expense A" },
                new Expense { Id = 2, Amount = 200, Description = "Expense B" },
            };
            _expenseRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expenses);

            // Act
            var result = await _expenseService.GetAllExpensesAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<IEnumerable<ExpenseDTO>>());
                Assert.That(result.Count, Is.EqualTo(2));
            });
        }

        [Test]
        public async Task GetExpenseByIdAsync_ShouldReturnExpenseWithGivenId()
        {
            // Arrange
            int testExpenseId = 1;
            var expectedExpense = new Expense { Id = testExpenseId, Amount = 100, Description = "Expense A" };
            _expenseRepositoryMock.Setup(repo => repo.GetByIdAsync(testExpenseId)).ReturnsAsync(expectedExpense);

            // Act
            var result = await _expenseService.GetExpenseByIdAsync(testExpenseId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(testExpenseId));
            });
        }

        [Test]
        public async Task CreateExpenseAsync_ShouldCreateExpenseSuccessfully()
        {
            // Arrange
            var expenseDto = new ExpenseDTO { Amount = 100, Description = "Expense A" };

            // Act
            var result = await _expenseService.CreateExpenseAsync(expenseDto);

            // Assert
            _expenseRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Expense>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateExpenseAsync_ShouldUpdateExpenseSuccessfully()
        {
            // Arrange
            var expenseDto = new ExpenseDTO { Id = 1, Amount = 150, Description = "Updated Expense A" };
            var existingExpense = new Expense { Id = 1, Amount = 100, Description = "Expense A" };
            _expenseRepositoryMock.Setup(repo => repo.GetByIdAsync(expenseDto.Id)).ReturnsAsync(existingExpense);

            // Act
            await _expenseService.UpdateExpenseAsync(expenseDto);

            // Assert
            _expenseRepositoryMock.Verify(repo => repo.Update(It.Is<Expense>(e => e.Amount == expenseDto.Amount && e.Description == expenseDto.Description)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteExpenseAsync_ShouldRemoveExpenseSuccessfully()
        {
            // Arrange
            int testExpenseId = 1;
            var existingExpense = new Expense { Id = testExpenseId, Amount = 100, Description = "Expense A" };
            _expenseRepositoryMock.Setup(repo => repo.GetByIdAsync(testExpenseId)).ReturnsAsync(existingExpense);

            // Act
            await _expenseService.DeleteExpenseAsync(testExpenseId);

            // Assert
            _expenseRepositoryMock.Verify(repo => repo.RemoveAsync(testExpenseId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Test]
        public void GetExpenseByIdAsync_ShouldThrowNotFoundException_WhenExpenseNotFound()
        {
            // Arrange
            int testExpenseId = 1;
            _expenseRepositoryMock.Setup(repo => repo.GetByIdAsync(testExpenseId)).Returns(Task.FromResult<Expense?>(null));

            // Act & Assert
            Assert.ThrowsAsync<FinanceManagementException>(() => _expenseService.GetExpenseByIdAsync(testExpenseId));
        }
    }
}
