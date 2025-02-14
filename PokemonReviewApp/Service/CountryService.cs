using System.Text.RegularExpressions;
using AutoMapper;
using FluentValidation;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Helper.Validators;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Interfaces.Services;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;


namespace PokemonReviewApp.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CountryInputModel> _countryValidator;

        public CountryService(ICountryRepository countryRepository, IMapper mapper, IValidator<CountryInputModel> countryValidator)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _countryValidator = countryValidator;
        }

        public async Task<ICollection<CountryOutputModel>> GetCountries()
        {
            var countries = await _countryRepository.GetCountries();
            return _mapper.Map<List<CountryOutputModel>>(countries);
        }

        public async Task<CountryOutputModel> GetCountryById(int countryId)
        {
            var country = await _countryRepository.GetCountry(countryId);
            return _mapper.Map<CountryOutputModel>(country);
        }

        public async Task<CountryOutputModel> GetCountryByOwner(int ownerId)
        {
            var country = await _countryRepository.GetCountryByOwner(ownerId);
            return _mapper.Map<CountryOutputModel>(country);
        }

        public async Task<bool> CountryExists(int countryId)
        {
            return await _countryRepository.CountryExists(countryId);
        }

        public async Task<CountryOutputModel> CreateCountry(CountryInputModel countryInputModel)
        {

            countryInputModel.Name = Regex.Replace(countryInputModel.Name.Trim(), @"\s+", " ").ToLower();

            var validationResult = await _countryValidator.ValidateAsync(countryInputModel);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Country data is not valid: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            bool countryAlrearyExists = await _countryRepository.CountryNameAlreadyExists(countryInputModel.Name);
            if (countryAlrearyExists)
            {
                throw new ArgumentException("A Country with the same name already exists.");
            }
            
            var country = _mapper.Map<Country>(countryInputModel);
            var createdCountry = await _countryRepository.CreateCountry(country);
            return _mapper.Map<CountryOutputModel>(createdCountry);

   
        }

        public async Task<CountryOutputModel> UpdateCountry(int id, CountryInputModel countryInputModel)
        {

            var existingCountry = await _countryRepository.GetCountry(id);
            if (existingCountry == null)
            {
                throw new ArgumentException("Country not found.");
            }

            countryInputModel.Name = Regex.Replace(countryInputModel.Name.Trim(), @"\s+", " ").ToLower();

            var validationResult = await _countryValidator.ValidateAsync(countryInputModel);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Country data is not valid: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            bool categoryAlreadyExists = await _countryRepository.CountryNameAlreadyExists(countryInputModel.Name);
            if (categoryAlreadyExists && countryInputModel.Name != existingCountry.Name)
            {
                throw new ArgumentException("A Country with the same name already exists.");
            }
            existingCountry.Name = countryInputModel.Name;

            var updatedCategory = await _countryRepository.UpdateCountry(existingCountry);
            return _mapper.Map<CountryOutputModel>(updatedCategory);
        }
    }
}
