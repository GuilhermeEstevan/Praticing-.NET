using AutoMapper;
using FluentValidation;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Interfaces.Services;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryInputModel> _categoryValidator;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IValidator<CategoryInputModel> categoryValidator)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _categoryValidator = categoryValidator;
        }

        public async Task<CategoryOutputModel> CreateCategory(CategoryInputModel categoryInputModel)
        {
            categoryInputModel.Name = categoryInputModel.Name.Trim().ToUpper();

            var validationResult = await _categoryValidator.ValidateAsync(categoryInputModel);

            if (!validationResult.IsValid) 
            { 
                throw new ArgumentException("Category data is not valid: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            bool categoryAlreadyExists = await _categoryRepository.CategoryNameAlreadyExists(categoryInputModel.Name);
            if (categoryAlreadyExists)
            {
                throw new ArgumentException("A category with the same name already exists.");
            }

            var category = _mapper.Map<Category>(categoryInputModel);
            var createdCategory = await _categoryRepository.CreateCategory(category);
            return _mapper.Map<CategoryOutputModel>(createdCategory);
        }

        public async Task<ICollection<CategoryOutputModel>> GetCategories()
        {
            var categories = await _categoryRepository.GetCategories();
            return _mapper.Map<ICollection<CategoryOutputModel>>(categories);
        }

        public async Task<CategoryOutputModel> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }

            return _mapper.Map<CategoryOutputModel>(category);
        }

        public async Task<ICollection<PokemonOutputModel>> GetPokemonsByCategory(int categoryId)
        {
            var pokemons = await _categoryRepository.GetPokemonsByCategory(categoryId);

            if (pokemons == null || !pokemons.Any())
            {
                throw new KeyNotFoundException($"No Pokémon found for Category with ID {categoryId}.");
            }

            return _mapper.Map<ICollection<PokemonOutputModel>>(pokemons);
        }

        public async Task<CategoryOutputModel> UpdateCategory(int id,CategoryInputModel categoryInputModel)
        {
            var existingCategory = await _categoryRepository.GetCategoryById(id);
            if (existingCategory == null)
            {
                throw new ArgumentException("Category not found.");
            }

            categoryInputModel.Name = categoryInputModel.Name.Trim().ToUpper();
            
            var validationResult = await _categoryValidator.ValidateAsync(categoryInputModel);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Category data is not valid: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            bool categoryAlreadyExists = await _categoryRepository.CategoryNameAlreadyExists(categoryInputModel.Name);
            if (categoryAlreadyExists && existingCategory.Name != categoryInputModel.Name)
            {
                throw new ArgumentException("A category with the same name already exists.");
            }

            existingCategory.Name = categoryInputModel.Name;

            var updatedCategory = await _categoryRepository.UpdateCategory(existingCategory);
            return _mapper.Map<CategoryOutputModel>(updatedCategory);
        }
    }
}