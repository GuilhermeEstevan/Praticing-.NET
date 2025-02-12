using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Repositories;


namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository CategoryRepository, IMapper mapper ) 
        {
            this._categoryRepository = CategoryRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetCategories();

                var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

                return Ok(categoriesDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            try
            {
                var categoryExists = await _categoryRepository.CategoryExists(categoryId);
                if (!categoryExists)
                {
                    return NotFound($"Category with ID {categoryId} not found.");
                }

                var category = await _categoryRepository.GetCategoryById(categoryId);
                var categoryDto = _mapper.Map<CategoryDto>(category);

                return Ok(categoryDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPokemonByCategoryId(int categoryId)
        {
            var categoryExists = await _categoryRepository.CategoryExists(categoryId);
            if (!categoryExists)
                return NotFound($"Category with ID {categoryId} not found.");

            var pokemons = await _categoryRepository.GetPokemonsByCategory(categoryId);
            var pokemonsDto = _mapper.Map<List<PokemonDto>>(pokemons);

            return Ok(pokemonsDto);
        }

    }
}
