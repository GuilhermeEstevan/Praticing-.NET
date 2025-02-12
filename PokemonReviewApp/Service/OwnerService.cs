using AutoMapper;
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

        public OwnerService(IOwnerRepository ownerRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
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
    }
}
