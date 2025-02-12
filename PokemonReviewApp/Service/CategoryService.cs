using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Interfaces.Services;

namespace PokemonReviewApp.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<CategoryOutputModel>> GetCategories()
        {
            var categories = await _categoryRepository.GetCategories();
            return _mapper.Map<ICollection<CategoryOutputModel>>(categories);
        }

        public async Task<CategoryOutputModel> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            return _mapper.Map<CategoryOutputModel>(category);
        }

        public async Task<ICollection<PokemonOutputModel>> GetPokemonsByCategory(int categoryId)
        {
            var pokemons = await _categoryRepository.GetPokemonsByCategory(categoryId);
            return _mapper.Map<ICollection<PokemonOutputModel>>(pokemons);
        }
    }
}