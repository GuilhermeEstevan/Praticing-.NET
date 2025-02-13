using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonOwnerRepository : IPokemonOwnerRepository
    {
        private readonly DataContext _context;

        public PokemonOwnerRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddPokemonOwner(int pokemonId, int ownerId)
        {
            var pokemonOwner = new PokemonOwner
            {
                PokemonId = pokemonId,
                OwnerId = ownerId
            };

            _context.PokemonOwners.Add(pokemonOwner);
            await _context.SaveChangesAsync();
        }
    }
}
