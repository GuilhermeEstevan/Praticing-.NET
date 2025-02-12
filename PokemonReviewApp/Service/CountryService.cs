using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Interfaces.Services;

namespace PokemonReviewApp.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryService(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<CountryOutputModel>> GetCountriesAsync()
        {
            var countries = await _countryRepository.GetCountries();
            return _mapper.Map<List<CountryOutputModel>>(countries);
        }

        public async Task<CountryOutputModel> GetCountryByIdAsync(int countryId)
        {
            var country = await _countryRepository.GetCountry(countryId);
            return _mapper.Map<CountryOutputModel>(country);
        }

        public async Task<CountryOutputModel> GetCountryByOwnerAsync(int ownerId)
        {
            var country = await _countryRepository.GetCountryByOwner(ownerId);
            return _mapper.Map<CountryOutputModel>(country);
        }

        public async Task<bool> CountryExistsAsync(int countryId)
        {
            return await _countryRepository.CountryExists(countryId);
        }
    }
}
