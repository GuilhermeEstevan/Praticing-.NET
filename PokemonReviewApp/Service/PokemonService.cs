using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Interfaces.Services;

namespace PokemonReviewApp.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonService(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<PokemonDto>> GetPokemons()
        {
            var pokemons = await _pokemonRepository.GetPokemons();
            return _mapper.Map<List<PokemonDto>>(pokemons);
        }

        public async Task<PokemonDto> GetPokemonById(int id)
        {
            var pokemon = await _pokemonRepository.GetPokemonById(id);
            return _mapper.Map<PokemonDto>(pokemon);
        }

        public async Task<decimal> GetPokemonRating(int pokemonId)
        {
            return await _pokemonRepository.GetPokemonRating(pokemonId);
        }

        public async Task<bool> PokemonExist(int id)
        {
            return await _pokemonRepository.PokemonExist(id);
        }

        public async Task<PokemonDto> GetPokemonByName(string name)
        {
            var pokemon = await _pokemonRepository.GetPokemonByName(name);
            return _mapper.Map<PokemonDto>(pokemon);
        }
    }
}