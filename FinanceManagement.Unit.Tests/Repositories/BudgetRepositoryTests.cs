using FinanceManagement.Core;
using FinanceManagement.DAL.Data;
using FinanceManagement.DAL.Repositories.Implementations;
using FinanceManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagement.Unit.Tests.Repositories
{
    public class BudgetRepositoryTests
    {
        private IBudgetRepository _budgetRepository;
        private FinanceDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FinanceDbContext>()
                .UseInMemoryDatabase(databaseName: "TestBudgetDbContext")
                .Options;

            _context = new FinanceDbContext(options);
            _budgetRepository = new BudgetRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task AddBudget_ShouldAddBudgetToRepository()
        {
            // Arrange
            var budget = new Budget { UserId = "1", Name = "Budget A", Amount = 500, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1) };

            // Act
            await _budgetRepository.AddAsync(budget);
            await _context.SaveChangesAsync();
            var allBudgets = await _budgetRepository.GetAllAsync();
            var savedBudget = await _budgetRepository.GetByIdAsync(budget.Id);

            // Assert
            Assert.Multiple(() => {
                Assert.That(savedBudget, Is.Not.Null);
                Assert.That(allBudgets.Count(), Is.EqualTo(1));
            });
        }

        [Test]
        public async Task UpdateBudget_ShouldUpdateBudgetInRepository()
        {
            // Arrange
            var budget = new Budget { UserId = "1", Name = "Budget A", Amount = 500, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1) };
            await _budgetRepository.AddAsync(budget);
            await _context.SaveChangesAsync();

            budget.Name = "Updated Budget A";
            _budgetRepository.Update(budget);

            // Act
            await _context.SaveChangesAsync();
            var updatedBudget = await _budgetRepository.GetByIdAsync(budget.Id);

            // Assert
            Assert.Multiple(() => {
                Assert.That(updatedBudget, Is.Not.Null);
                Assert.That(updatedBudget?.Name, Is.EqualTo("Updated Budget A"));
            });
        }

        [Test]
        public async Task RemoveAsync_ShouldRemoveBudgetFromRepository()
        {
            // Arrange
            var budget = new Budget { UserId = "1", Name = "Budget A", Amount = 500, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1) };
            await _budgetRepository.AddAsync(budget);
            await _context.SaveChangesAsync();

            // Act
            await _budgetRepository.RemoveAsync(budget.Id);
            await _context.SaveChangesAsync();
            var deletedBudget = await _budgetRepository.GetByIdAsync(budget.Id);

            // Assert
            Assert.That(deletedBudget, Is.Null);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllBudgetsFromRepository()
        {
            // Arrange
            var budgets = new List<Budget>
            {
                new Budget { UserId = "1", Name = "Budget A", Amount = 500, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1) },
                new Budget { UserId = "1", Name = "Budget B", Amount = 300, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1) }
            };

            await _context.AddRangeAsync(budgets);
            await _context.SaveChangesAsync();

            // Act
            var result = await _budgetRepository.GetAllAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(2));
            });
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnBudgetWithGivenIdFromRepository()
        {
            // Arrange
            var budget = new Budget { UserId = "1", Name = "Budget A", Amount = 500, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1) };
            await _budgetRepository.AddAsync(budget);
            await _context.SaveChangesAsync();

            // Act
            var result = await _budgetRepository.GetByIdAsync(budget.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.Multiple(() =>
                {
                    Assert.That(result?.Amount, Is.EqualTo(budget.Amount));
                    Assert.That(result?.Id, Is.EqualTo(budget.Id));
                });
            });
        }
    }
}
