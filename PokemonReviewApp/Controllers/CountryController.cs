using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Services;

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
                var countries = await _countryService.GetCountriesAsync();
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
                var countryExists = await _countryService.CountryExistsAsync(countryId);
                if (!countryExists)
                {
                    return NotFound($"Country with ID {countryId} not found.");
                }

                var country = await _countryService.GetCountryByIdAsync(countryId);
                return Ok(country);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpGet("pokemonOwner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(CountryOutputModel))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCountryByOwner(int ownerId)
        {
            try
            {
                var country = await _countryService.GetCountryByOwnerAsync(ownerId);
                if (country == null)
                {
                    return NotFound($"Country for Owner with ID {ownerId} not found.");
                }
                return Ok(country);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }
    }
}
