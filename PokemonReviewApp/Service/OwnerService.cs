using AutoMapper;
using FluentValidation;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Interfaces.Services;
using PokemonReviewApp.Models;



namespace PokemonReviewApp.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<OwnerInputModel> _ownerValidator;
        private readonly ICountryRepository _countryRepository;

        public OwnerService(
             IOwnerRepository ownerRepository,
             IMapper mapper,
             IValidator<OwnerInputModel> ownerValidator,
             ICountryRepository countryRepository
        )
        {
            this._ownerRepository = ownerRepository;
            this._mapper = mapper;
            this._ownerValidator = ownerValidator;
            this._countryRepository = countryRepository;
        }

        public async Task<ICollection<OwnerOutputModel>> GetOwners(int pageNumber, int pageSize)
        {
            var owners = await _ownerRepository.GetOwners(pageNumber, pageSize);
            return _mapper.Map<ICollection<OwnerOutputModel>>(owners);
        }

        public async Task<OwnerOutputModel> GetOwnerById(int ownerId)
        {
            var owner = await _ownerRepository.GetOwner(ownerId);
            if (owner == null) return null;
            return _mapper.Map<OwnerOutputModel>(owner);
        }

        public async Task<ICollection<OwnerOutputModel>> GetOwnersByPokemon(int pokemonId)
        {
            var owners = await _ownerRepository.GetOwnersByPokemon(pokemonId);
            return _mapper.Map<ICollection<OwnerOutputModel>>(owners);
        }

        public async Task<bool> OwnerExists(int ownerId)
        {
            return await _ownerRepository.OwnerExists(ownerId);
        }

        public async Task<ICollection<PokemonOutputModel>> GetPokemonsByOwner(int ownerId)
        {
            var pokemons = await _ownerRepository.GetPokemonsByOwner(ownerId);
            return _mapper.Map<List<PokemonOutputModel>>(pokemons);
        }

        public async Task<OwnerOutputModel> CreateOwner(OwnerInputModel ownerInputModel)
        {
            var cleanedFirstName = (ownerInputModel.FirstName.Trim().ToLower());
            var cleanedLastName = Utils.Utils.RemoveAccents(ownerInputModel.LastName.Trim().ToLower());
            var cleanedGymName = Utils.Utils.RemoveAccents(ownerInputModel.Gym.Trim().ToLower());
            ownerInputModel.FirstName = cleanedFirstName;
            ownerInputModel.LastName = cleanedLastName;
            ownerInputModel.Gym = cleanedGymName;

            var validationResult = await _ownerValidator.ValidateAsync(ownerInputModel);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Owner data is not valid: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var country = await _countryRepository.GetCountry(ownerInputModel.CountryId);
            if (country == null)
            {
                throw new ArgumentException("The specified country does not exist.");
            }

            var owner = _mapper.Map<Owner>(ownerInputModel);
            owner.Country = country;

            var createdOwner =await _ownerRepository.CreateOwner(owner);
            return _mapper.Map<OwnerOutputModel>(createdOwner);

        }
    }
}
