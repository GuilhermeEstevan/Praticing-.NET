using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Interfaces.Services;

namespace PokemonReviewApp.Service
{
    public class PokemonOwnerService : IPokemonOwnerService
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPokemonOwnerRepository _pokemonOwnerRepository;

        public PokemonOwnerService(IPokemonRepository pokemonRepository, IOwnerRepository ownerRepository, IPokemonOwnerRepository pokemonOwnerRepository)
        {
            _pokemonRepository = pokemonRepository;
            _ownerRepository = ownerRepository;
            _pokemonOwnerRepository = pokemonOwnerRepository;
        }

        public async Task CapturePokemon(int pokemonId, int ownerId)
        {
            // Verifica se o Pokémon existe
            var pokemon = await _pokemonRepository.GetPokemonById(pokemonId);
            if (pokemon == null)
            {
                throw new ArgumentException("Pokemon not found.");
            }

            // Verifica se o Owner existe
            var owner = await _ownerRepository.GetOwner(ownerId);
            if (owner == null)
            {
                throw new ArgumentException("Owner not found.");
            }

            await _pokemonOwnerRepository.AddPokemonOwner(pokemonId, ownerId);
        }
    }
}
