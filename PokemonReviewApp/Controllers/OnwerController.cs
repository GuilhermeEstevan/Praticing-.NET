using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Services;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _ownerService;

        public OwnerController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }

 
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerOutputModel>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOwners([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var owners = await _ownerService.GetOwners(pageNumber, pageSize);
                return Ok(owners);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

      
        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(OwnerOutputModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOwnerById(int ownerId)
        {
            try
            {
                var owner = await _ownerService.GetOwnerById(ownerId);
                if (owner == null)
                {
                    return NotFound($"Owner with ID {ownerId} not found.");
                }
                return Ok(owner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

     
        [HttpGet("{ownerId}/pokemons")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonOutputModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPokemonsByOwner(int ownerId)
        {
            try
            {
                var pokemons = await _ownerService.GetPokemonsByOwner(ownerId);
                return Ok(pokemons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpGet("pokemon/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerOutputModel>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOwnerByPokemon(int pokemonId)
        {
            try
            {
                var owners = await _ownerService.GetOwnersByPokemon(pokemonId);
                if (owners == null || !owners.Any())
                {
                    return NotFound($"No owners found for Pokemon with ID {pokemonId}");
                }
                return Ok(owners);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(OwnerOutputModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateOwner([FromBody] OwnerInputModel ownerInputModel)
        {
            if (ownerInputModel == null)
            {
                return BadRequest(new { error = "Invalid data." });
            }

            try
            {
                var createdOwner = await _ownerService.CreateOwner(ownerInputModel);
                return CreatedAtAction(nameof(GetOwnerById), new { ownerId = createdOwner.Id }, createdOwner);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while saving entity: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                return StatusCode(500, new { error = "Internal server error: " + ex.Message });
            }
        }

    }
}
