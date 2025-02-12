using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Repositories;


namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController :ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IOwnerRepository ownerRepository,IMapper mapper)
        {
            this._countryRepository = countryRepository;
            this._ownerRepository = ownerRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCountries()
        {
            try 
            {
                var countries = await _countryRepository.GetCountries();
                var countriesDto = _mapper.Map<List<CountryDto>>(countries);

                return Ok(countriesDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCategoryById(int countryId)
        {
            try
            {
                var countryExists = await _countryRepository.CountryExists(countryId);
                if (!countryExists)
                {
                    return NotFound($"Country with ID {countryId} not found.");
                }

                var country = await _countryRepository.GetCountry(countryId);
                var countryDto = _mapper.Map<CountryDto>(country);

                return Ok(countryDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpGet("owner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCountryByOwner(int ownerId)
        { 
           try
           {
                var ownerExists = await _ownerRepository.OwnerExists(ownerId);
                if (!ownerExists)
                    return NotFound($"Owner with ID {ownerId} not found.");

                var country = await _countryRepository.GetCountryByOwner(ownerId);
                if (country == null)
                    return NotFound($"Country for Owner with ID {ownerId} not found.");

                var countryDto = _mapper.Map<CountryDto>(country);
                return Ok(countryDto);
           }
            catch(Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }

        }
    }
}
