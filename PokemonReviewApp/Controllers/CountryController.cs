using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Services;
using PokemonReviewApp.Service;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
    

        public CountryController(ICountryService countryService )
        {
            _countryService = countryService;
         
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryOutputModel>))]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _countryService.GetCountries();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(CountryOutputModel))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCountryById(int countryId)
        {
            try
            {
                var country = await _countryService.GetCountryById(countryId);
                return Ok(country);
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

        [HttpGet("pokemonOwner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(CountryOutputModel))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCountryByOwner(int ownerId)
        {
            try
            {
                var country = await _countryService.GetCountryByOwner(ownerId);
                return Ok(country);
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
        [ProducesResponseType(201, Type = typeof(CountryOutputModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCategory([FromBody] CountryInputModel countryInputModel)
        {
            if (countryInputModel == null)
            {
                return BadRequest(new { message = "Invalid data." });
            }

            try
            {
                var createdCountry = await _countryService.CreateCountry(countryInputModel);
                return CreatedAtAction(nameof(GetCountryById), new { countryId = createdCountry.Id }, createdCountry);

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

        // PUT api/country?id={id}
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(CountryOutputModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CategoryOutputModel>> UpdateCountry([FromQuery] int id, [FromBody] CountryInputModel countryInputModel)
        {
            try
            {
                var updatedCountry = await _countryService.UpdateCountry(id, countryInputModel);

                return Ok(updatedCountry);
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

        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)] // No Content
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCountry(int countryId)
        {
            try
            {
                var deleted = await _countryService.DeleteCountry(countryId);
                if (!deleted)
                {
                    return NotFound(new { error = "Country not found." });
                }

                return NoContent(); // Retorna 204 - No Content
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
