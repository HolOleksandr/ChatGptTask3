using AutoMapper;
using FinanceManagement.BLL.Exceptions;
using FinanceManagement.BLL.Services.Interfaces;
using FinanceManagement.Core;
using FinanceManagement.DAL.Repositories.Interfaces;
using FinanceManagement.DAL.UnitOfWork.Interfaces;
using FinanceManagement.Models.DTOs;

namespace FinanceManagement.BLL.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.GetRepository<ICategoryRepository>().GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var category = await GetCategoryOrThrowAsync(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _unitOfWork.GetRepository<ICategoryRepository>().AddAsync(category);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task UpdateCategoryAsync(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _unitOfWork.GetRepository<ICategoryRepository>().Update(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryOrThrowAsync(id);
            await _unitOfWork.GetRepository<ICategoryRepository>().RemoveAsync(category.Id);
            await _unitOfWork.SaveAsync();
        }

        private async Task<Category> GetCategoryOrThrowAsync(int id)
        {
            var category = await _unitOfWork.GetRepository<ICategoryRepository>().GetByIdAsync(id);

            return category ?? throw new FinanceManagementException($"Category with ID {id} not found");
        }
    }
}
