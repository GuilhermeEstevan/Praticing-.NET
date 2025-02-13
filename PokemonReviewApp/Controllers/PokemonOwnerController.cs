
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Services;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonOwnerController : ControllerBase
    {
        private readonly IPokemonOwnerService _pokemonOwnerService;

        public PokemonOwnerController(IPokemonOwnerService pokemonOwnerService)
        {
            this._pokemonOwnerService = pokemonOwnerService;
        }

        [HttpPost("capture")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CapturePokemon([FromBody] PokemonOwnerInputModel pokemonOwnerInputModel)
        {

            try
            {
                await _pokemonOwnerService.CapturePokemon(pokemonOwnerInputModel.PokemonId, pokemonOwnerInputModel.OwnerId);
                return Ok("Pokemon captured successfully.");
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

    }
}
