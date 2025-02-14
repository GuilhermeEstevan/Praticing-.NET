using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Services;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonOutputModel>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPokemons()
        {
            try
            {
                var pokemons = await _pokemonService.GetPokemons();
                return Ok(pokemons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Erro interno no servidor: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PokemonOutputModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPokemonById(int id)
        {
            try
            {
                var pokemon = await _pokemonService.GetPokemonById(id);
                return Ok(pokemon);
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

        [HttpGet("{id}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPokemonRating(int id)
        {
            try
            {
                var rating = await _pokemonService.GetPokemonRating(id);
                return Ok(rating);
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
        [ProducesResponseType(201, Type = typeof(PokemonOutputModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePokemon([FromBody] PokemonInputModel pokemonInputModel)
        {
            try
            {
                var createdPokemon = await _pokemonService.CreatePokemon(pokemonInputModel);
                return CreatedAtAction(nameof(GetPokemonById), new { id = createdPokemon.Id }, createdPokemon);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(PokemonOutputModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PokemonOutputModel>> UpdatePokemon([FromQuery] int id, [FromBody] PokemonUpdateModel pokemonUpdateModel)
        {
            try
            {
                var updatedPokemon = await _pokemonService.UpdatePokemon(id, pokemonUpdateModel);
                return Ok(updatedPokemon);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePokemon([FromQuery] int id)
        {
            try
            {
                var deleted = await _pokemonService.DeletePokemon(id);
                if (!deleted)
                {
                    return NotFound(new { error = "Pokemon not found." });
                }

                return NoContent(); 
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }
    }
}
