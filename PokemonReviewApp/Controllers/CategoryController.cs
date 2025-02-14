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
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Erro interno no servidor: {ex.Message}" });
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
                return Ok(pokemons);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Erro interno no servidor: {ex.Message}" });
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

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)] 
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                var deleted = await _categoryService.DeleteCategory(categoryId);
                if (!deleted)
                {
                    return NotFound(new { error = "Category not found." });
                }

                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }
    }
}