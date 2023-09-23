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
    public class CategoryServiceTests
    {
        private CategoryService _categoryService;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock.Setup(uow => uow.GetRepository<ICategoryRepository>()).Returns(_categoryRepositoryMock.Object);

            _categoryService = new CategoryService(_unitOfWorkMock.Object, mapper);
        }

        [Test]
        public async Task GetAllCategoriesAsync_ShouldReturnAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category A" },
                new Category { Id = 2, Name = "Category B" },
            };
            _categoryRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllCategoriesAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<IEnumerable<CategoryDTO>>());
                Assert.That(result.Count, Is.EqualTo(2));
            });
        }

        [Test]
        public async Task GetCategoryByIdAsync_ShouldReturnCategoryWithGivenId()
        {
            // Arrange
            int testCategoryId = 1;
            var expectedCategory = new Category { Id = testCategoryId, Name = "Category A" };
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(testCategoryId)).ReturnsAsync(expectedCategory);

            // Act
            var result = await _categoryService.GetCategoryByIdAsync(testCategoryId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(testCategoryId));
            });
        }

        [Test]
        public async Task CreateCategoryAsync_ShouldCreateCategorySuccessfully()
        {
            // Arrange
            var categoryDto = new CategoryDTO { Name = "Category A" };

            // Act
            var result = await _categoryService.CreateCategoryAsync(categoryDto);

            // Assert
            _categoryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateCategoryAsync_ShouldUpdateCategorySuccessfully()
        {
            // Arrange
            var categoryDto = new CategoryDTO { Id = 1, Name = "Updated Category A" };
            var existingCategory = new Category { Id = 1, Name = "Category A" };
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryDto.Id)).ReturnsAsync(existingCategory);

            // Act
            await _categoryService.UpdateCategoryAsync(categoryDto);

            // Assert
            _categoryRepositoryMock.Verify(repo => repo.Update(It.Is<Category>(c => c.Name == categoryDto.Name)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteCategoryAsync_ShouldRemoveCategorySuccessfully()
        {
            // Arrange
            int testCategoryId = 1;
            var existingCategory = new Category { Id = testCategoryId, Name = "Category A" };
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(testCategoryId)).ReturnsAsync(existingCategory);

            // Act
            await _categoryService.DeleteCategoryAsync(testCategoryId);

            // Assert
            _categoryRepositoryMock.Verify(repo => repo.RemoveAsync(testCategoryId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Test]
        public void GetCategoryByIdAsync_ShouldThrowNotFoundException_WhenCategoryNotFound()
        {
            // Arrange
            int testCategoryId = 1;
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(testCategoryId)).Returns(Task.FromResult<Category?>(null));

            // Act & Assert
            Assert.ThrowsAsync<FinanceManagementException>(() => _categoryService.GetCategoryByIdAsync(testCategoryId));
        }
    }
}
