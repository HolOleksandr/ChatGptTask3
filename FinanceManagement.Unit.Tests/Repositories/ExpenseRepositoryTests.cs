using FinanceManagement.Core;
using FinanceManagement.DAL.Data;
using FinanceManagement.DAL.Repositories.Implementations;
using FinanceManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Unit.Tests.Repositories
{
    public class ExpenseRepositoryTests
    {
        private IExpenseRepository _expenseRepository;
        private FinanceDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FinanceDbContext>()
                .UseInMemoryDatabase(databaseName: "TestExpenseDbContext")
                .Options;

            _context = new FinanceDbContext(options);
            _expenseRepository = new ExpenseRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task AddExpense_ShouldAddExpenseToRepository()
        {
            // Arrange
            var expense = new Expense { Description = "Expense A", Amount = 100, Date = System.DateTime.Now, CategoryId = 1, UserId = "1" };

            // Act
            await _expenseRepository.AddAsync(expense);
            await _context.SaveChangesAsync();
            var allExpenses = await _expenseRepository.GetAllAsync();
            var savedExpense = await _expenseRepository.GetByIdAsync(expense.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(savedExpense, Is.Not.Null);
                Assert.That(allExpenses.Count(), Is.EqualTo(1));
            });
        }

        [Test]
        public async Task UpdateExpense_ShouldUpdateExpenseInRepository()
        {
            // Arrange
            var expense = new Expense { Description = "Expense A", Amount = 100, Date = System.DateTime.Now, CategoryId = 1, UserId = "1" };
            await _expenseRepository.AddAsync(expense);
            await _context.SaveChangesAsync();

            expense.Description = "Updated Expense A";
            _expenseRepository.Update(expense);

            // Act
            await _context.SaveChangesAsync();
            var updatedExpense = await _expenseRepository.GetByIdAsync(expense.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(updatedExpense, Is.Not.Null);
                Assert.That(updatedExpense?.Description, Is.EqualTo("Updated Expense A"));
            });
        }

        [Test]
        public async Task RemoveAsync_ShouldRemoveExpenseFromRepository()
        {
            // Arrange
            var expense = new Expense { Description = "Expense A", Amount = 100, Date = System.DateTime.Now, CategoryId = 1, UserId = "1" };
            await _expenseRepository.AddAsync(expense);
            await _context.SaveChangesAsync();

            // Act
            await _expenseRepository.RemoveAsync(expense.Id);
            await _context.SaveChangesAsync();
            var deletedExpense = await _expenseRepository.GetByIdAsync(expense.Id);

            // Assert
            Assert.That(deletedExpense, Is.Null);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllExpensesFromRepository()
        {
            // Arrange
            var expenses = new List<Expense>
            {
                new Expense { Description = "Expense A", Amount = 100, Date = System.DateTime.Now, CategoryId = 1, UserId = "1" },
                new Expense { Description = "Expense B", Amount = 200, Date = System.DateTime.Now, CategoryId = 2, UserId = "1" }
            };

            await _context.AddRangeAsync(expenses);
            await _context.SaveChangesAsync();

            // Act
            var result = await _expenseRepository.GetAllAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(2));
            });
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnExpenseWithGivenIdFromRepository()
        {
            // Arrange
            var expense = new Expense { Description = "Expense A", Amount = 100, Date = System.DateTime.Now, CategoryId = 1, UserId = "1" };
            await _expenseRepository.AddAsync(expense);
            await _context.SaveChangesAsync();

            // Act
            var result = await _expenseRepository.GetByIdAsync(expense.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result?.Amount, Is.EqualTo(expense.Amount));
                Assert.That(result?.Id, Is.EqualTo(expense.Id));
            });
        }
    }
}
