using FinanceManagement.Core;
using FinanceManagement.DAL.Data;
using FinanceManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Unit.Tests.Repositories
{
    public class TransactionRepositoryTests
    {
        private Mock<ITransactionRepository> _transactionRepository;
        private FinanceDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FinanceDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDbContext")
                .Options;

            _context = new FinanceDbContext(options);
            _transactionRepository = new Mock<ITransactionRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task AddTransaction_ShouldAddTransactionToRepository()
        {
            // Arrange
            var transaction = new FinancialTransaction() { UserId = "1", Amount = 20, Description = "Description" };
            _transactionRepository.Setup(r => r.AddAsync(transaction))
                .Callback(() => _context.AddAsync(transaction).AsTask())
                .Returns(Task.CompletedTask);

            // Act
            await _transactionRepository.Object.AddAsync(transaction);
            await _context.SaveChangesAsync();
            var allTransactions = _context.Transactions.ToList();
            var savedTransaction = await _context.Transactions.FindAsync(transaction.Id);

            // Assert
            Assert.That(savedTransaction, Is.Not.Null);
            _transactionRepository.Verify(r => r.AddAsync(transaction), Times.Once());
        }

        [Test]
        public async Task UpdateTransaction_ShouldUpdateTransactionInRepository()
        {
            // Arrange
            var transaction = new FinancialTransaction() { UserId = "1", Amount = 20, Description = "Description" };
            await _context.AddAsync(transaction);
            await _context.SaveChangesAsync();

            transaction.Amount = 500;

            _ = _transactionRepository.Setup(r => r.Update(transaction))
                .Callback(() => _context.Entry(transaction).State = EntityState.Modified);

            // Act
            _transactionRepository.Object.Update(transaction);
            await _context.SaveChangesAsync();
            var updatedTransaction = await _context.Transactions.FindAsync(transaction.Id);

            // Assert
            Assert.That(updatedTransaction, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(updatedTransaction.Amount, Is.EqualTo(500));
            });
            _transactionRepository.Verify(r => r.Update(transaction), Times.Once());
        }

        [Test]
        public async Task RemoveAsync_ShouldRemoveTransactionFromRepository()
        {
            // Arrange
            var transaction = new FinancialTransaction() {UserId = "1", Amount = 20, Description = "Description" };
            await _context.AddAsync(transaction);
            await _context.SaveChangesAsync();

            var transactionId = transaction.Id;
            _ = _transactionRepository.Setup(r => r.RemoveAsync(transactionId))
                .Callback(async () =>
                {
                    var entity = await _context.Transactions.FindAsync(transactionId);
                    if (entity != null)
                    {
                        _ = _context.Transactions.Remove(entity);
                    }
                });

            // Act
            await _transactionRepository.Object.RemoveAsync(transactionId);
            await _context.SaveChangesAsync();
            var deletedTransaction = await _context.Transactions.FindAsync(transactionId);

            // Assert
            Assert.That(deletedTransaction, Is.Null);
            _transactionRepository.Verify(r => r.RemoveAsync(transactionId), Times.Once());
        }

        [Test]
        public async Task GetAllTransactionsAsync_ShouldReturnAllTransactionsFromRepository()
        {
            // Arrange
            var transactions = new List<FinancialTransaction>
            {
                new FinancialTransaction() {UserId = "1", Amount = 20, Description = "Description 1"},
                new FinancialTransaction() {UserId = "1", Amount = 20, Description = "Description 2"}
            };
            await _context.AddRangeAsync(transactions);
            await _context.SaveChangesAsync();

            _ = _transactionRepository.Setup(r => r.GetAllAsync())
                .Returns(async () => await _context.Transactions.ToListAsync());

            // Act
            var result = await _transactionRepository.Object.GetAllAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnTransactionWithGivenIdFromRepository()
        {
            // Arrange
            var transaction = new FinancialTransaction() { UserId = "1", Amount = 20, Description = "Description 1" };
            await _context.AddAsync(transaction);
            await _context.SaveChangesAsync();

            var transactionId = transaction.Id;
            _ = _transactionRepository.Setup(r => r.GetByIdAsync(transactionId))
                .Returns(async () => await _context.Transactions.FindAsync(transactionId));

            // Act
            var result = await _transactionRepository.Object.GetByIdAsync(transactionId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Amount, Is.EqualTo(transaction.Amount));
                Assert.That(result.Id, Is.EqualTo(transaction.Id));
            });
        }
    }
}
