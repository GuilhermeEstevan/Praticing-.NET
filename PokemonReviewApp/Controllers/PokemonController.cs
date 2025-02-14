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
        public async Task<IActionResult> GetPokemons()
        {
            try
            {
                var pokemons = await _pokemonService.GetPokemons();
                return Ok(pokemons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PokemonOutputModel))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPokemonById(int id)
        {
            try
            {
                var pokemon = await _pokemonService.GetPokemonById(id);
                if (pokemon == null)
                {
                    return NotFound($"Pokemon with ID {id} not found.");
                }

                return Ok(pokemon);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPokemonRating(int id)
        {
            try
            {
                var pokemonExists = await _pokemonService.PokemonExist(id);
                if (!pokemonExists)
                    return NotFound($"Pokemon with ID {id} not found.");

                var rating = await _pokemonService.GetPokemonRating(id);
                return Ok(rating);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
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
                return StatusCode(500, new { error = "Internal server error: " + ex.Message });
            }
        }

        // PUT api/pokemon?id={id}
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(PokemonOutputModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
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
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
