using FinanceManagement.Core;
using FinanceManagement.DAL.Data;
using FinanceManagement.DAL.Repositories.Implementations;
using FinanceManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagement.Unit.Tests.Repositories
{
    public class CategoryRepositoryTests
    {
        private ICategoryRepository _categoryRepository;
        private FinanceDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FinanceDbContext>()
                .UseInMemoryDatabase(databaseName: "TestCategoryDbContext")
                .Options;

            _context = new FinanceDbContext(options);
            _categoryRepository = new CategoryRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task AddCategory_ShouldAddCategoryToRepository()
        {
            // Arrange
            var category = new Category { Name = "Category A", BudgetId = 1 };

            // Act
            await _categoryRepository.AddAsync(category);
            await _context.SaveChangesAsync();
            var allCategories = await _categoryRepository.GetAllAsync();
            var savedCategory = await _categoryRepository.GetByIdAsync(category.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(savedCategory, Is.Not.Null);
                Assert.That(allCategories.Count(), Is.EqualTo(1));
            });
        }

        [Test]
        public async Task UpdateCategory_ShouldUpdateCategoryInRepository()
        {
            // Arrange
            var category = new Category { Name = "Category A", BudgetId = 1 };
            await _categoryRepository.AddAsync(category);
            await _context.SaveChangesAsync();

            category.Name = "Updated Category A";
            _categoryRepository.Update(category);

            // Act
            await _context.SaveChangesAsync();
            var updatedCategory = await _categoryRepository.GetByIdAsync(category.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(updatedCategory, Is.Not.Null);
                Assert.That(updatedCategory?.Name, Is.EqualTo("Updated Category A"));
            });
        }

        [Test]
        public async Task RemoveAsync_ShouldRemoveCategoryFromRepository()
        {
            // Arrange
            var category = new Category { Name = "Category A", BudgetId = 1 };
            await _categoryRepository.AddAsync(category);
            await _context.SaveChangesAsync();

            // Act
            await _categoryRepository.RemoveAsync(category.Id);
            await _context.SaveChangesAsync();
            var deletedCategory = await _categoryRepository.GetByIdAsync(category.Id);

            // Assert
            Assert.That(deletedCategory, Is.Null);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllCategoriesFromRepository()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Name = "Category A", BudgetId = 1 },
                new Category { Name = "Category B", BudgetId = 2 }
            };

            await _context.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            // Act
            var result = await _categoryRepository.GetAllAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(2));
            });
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCategoryWithGivenIdFromRepository()
        {
            // Arrange
            var category = new Category { Name = "Category A", BudgetId = 1 };
            await _categoryRepository.AddAsync(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _categoryRepository.GetByIdAsync(category.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result?.Id, Is.EqualTo(category.Id));
                Assert.That(result?.Name, Is.EqualTo(category.Name));
            });
        }
    }
}
