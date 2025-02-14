using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Services;


namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryOutputModel>))]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(CategoryOutputModel))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(categoryId);
                if (category == null)
                    return NotFound($"Category with ID {categoryId} not found.");

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonOutputModel>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPokemonByCategoryId(int categoryId)
        {
            try
            {
                var pokemons = await _categoryService.GetPokemonsByCategory(categoryId);
                if (pokemons == null || !pokemons.Any())
                    return NotFound($"No Pokémon found for Category with ID {categoryId}.");

                return Ok(pokemons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoryOutputModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryInputModel categoryInputModel)
        {
            if (categoryInputModel == null)
            {
                return BadRequest(new { message = "Invalid data." });
            }

            try
            {
                var createdCategory = await _categoryService.CreateCategory(categoryInputModel);
                return CreatedAtAction(nameof(GetCategoryById), new { categoryId = createdCategory.Id }, createdCategory);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // PUT api/category?id={id}
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(CategoryOutputModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CategoryOutputModel>> UpdateCategory([FromQuery] int id, [FromBody] CategoryInputModel categoryInputModel)
        {
            try
            { 
                var updatedCategory = await _categoryService.UpdateCategory(id,categoryInputModel);

                return Ok(updatedCategory); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}