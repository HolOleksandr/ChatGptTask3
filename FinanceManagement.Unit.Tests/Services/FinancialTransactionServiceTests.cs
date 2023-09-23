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
    public class FinancialTransactionServiceTests
    {
        private FinancialTransactionService _financialTransactionService;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ITransactionRepository> _transactionRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _unitOfWorkMock.Setup(uow => uow.GetRepository<ITransactionRepository>()).Returns(_transactionRepositoryMock.Object);

            _financialTransactionService = new FinancialTransactionService(_unitOfWorkMock.Object, mapper);
        }

        [Test]
        public async Task GetAllTransactionsAsync_ShouldReturnAllTransactions()
        {
            // Arrange
            var transactions = new List<FinancialTransaction>
            {
                new FinancialTransaction { Id = 1, Description = "Transaction A", Amount = 100 },
                new FinancialTransaction { Id = 2, Description = "Transaction B", Amount = 200 },
            };
            _transactionRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(transactions);

            // Act
            var result = await _financialTransactionService.GetAllTransactionsAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<IEnumerable<FinancialTransactionDTO>>());
                Assert.That(result.Count, Is.EqualTo(2));
            });
        }

        [Test]
        public async Task GetTransactionByIdAsync_ShouldReturnTransactionWithGivenId()
        {
            // Arrange
            int testTransactionId = 1;
            var expectedTransaction = new FinancialTransaction { Id = testTransactionId, Description = "Transaction A", Amount = 100 };
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(testTransactionId)).ReturnsAsync(expectedTransaction);

            // Act
            var result = await _financialTransactionService.GetTransactionByIdAsync(testTransactionId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(testTransactionId));
            });
        }

        [Test]
        public async Task CreateTransactionAsync_ShouldCreateTransactionSuccessfully()
        {
            // Arrange
            var transactionDto = new FinancialTransactionDTO { Description = "Transaction A", Amount = 100 };

            // Act
            var result = await _financialTransactionService.CreateTransactionAsync(transactionDto);

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<FinancialTransaction>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateTransactionAsync_ShouldUpdateTransactionSuccessfully()
        {
            // Arrange
            var transactionDto = new FinancialTransactionDTO { Id = 1, Description = "Updated Transaction A", Amount = 150 };
            var existingTransaction = new FinancialTransaction { Id = 1, Description = "Transaction A", Amount = 100 };
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(transactionDto.Id)).ReturnsAsync(existingTransaction);

            // Act
            await _financialTransactionService.UpdateTransactionAsync(transactionDto);

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.Update(It.Is<FinancialTransaction>(t => t.Amount == transactionDto.Amount && t.Description == transactionDto.Description)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteTransactionAsync_ShouldRemoveTransactionSuccessfully()
        {
            // Arrange
            int testTransactionId = 1;
            var existingTransaction = new FinancialTransaction { Id = testTransactionId, Description = "Transaction A", Amount = 100 };
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(testTransactionId)).ReturnsAsync(existingTransaction);

            // Act
            await _financialTransactionService.DeleteTransactionAsync(testTransactionId);

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.RemoveAsync(testTransactionId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Test]
        public void GetTransactionByIdAsync_ShouldThrowNotFoundException_WhenTransactionNotFound()
        {
            // Arrange
            int testTransactionId = 1;
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(testTransactionId)).Returns(Task.FromResult<FinancialTransaction?>(null));

            // Act & Assert
            Assert.ThrowsAsync<FinanceManagementException>(() => _financialTransactionService.GetTransactionByIdAsync(testTransactionId));
        }
    }
}
