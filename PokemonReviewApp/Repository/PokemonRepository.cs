using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context) 
        {
            this._context = context;
        }

        
        public async Task<ICollection<Pokemon>> GetPokemons()
        {
            
            return await _context.Pokemon.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<Pokemon> GetPokemonById(int id)
        {
            var pokemon = await _context.Pokemon
                                         .Where(p => p.Id == id)
                                         .Include(p => p.PokemonCategories)
                                         .ThenInclude(pc => pc.Category)
                                         .FirstOrDefaultAsync();
            if (pokemon == null)
            {
                throw new KeyNotFoundException($"Pokemon with ID {id} not found.");
            }
            return pokemon;
        }

        public async Task<decimal> GetPokemonRating(int pokemonId)
        {
            var review = await _context.Reviews.Where(r => r.Pokemon.Id == pokemonId).ToListAsync();

            if (review.Count <= 0)
            {
                return 0;
            }
            return Math.Round((decimal)review.Sum(r => r.Rating) / review.Count, 2);

        }
        public async Task<bool> PokemonExist(int id)
        {
            return await _context.Pokemon.AnyAsync(p => p.Id == id);
        }

        public async Task<Pokemon> GetPokemonByName(string name)
        {
            var pokemon = await _context.Pokemon
                                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                                .FirstOrDefaultAsync();

            if (pokemon == null)
            {
                throw new KeyNotFoundException($"Pokemon with name {name} not found.");
            }

            return pokemon;
        }

        public async Task<Pokemon> CreatePokemon(Pokemon pokemon)
        {
            _context.Pokemon.Add(pokemon);
            await _context.SaveChangesAsync();
            return pokemon;
        }

        public async Task<bool> PokemonNameAlreadyExists(string name)
        {
            return await _context.Pokemon
                                    .AnyAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task<Pokemon> UpdatePokemon(Pokemon pokemon)
        {
            _context.Pokemon.Update(pokemon);
            await _context.SaveChangesAsync();
            return pokemon;
        }

        public async Task<bool> DeletePokemon(int id)
        {
            var pokemon = await _context.Pokemon
                                        .Include(p => p.PokemonOwners)
                                        .Include(p => p.PokemonCategories)
                                        .Include(p => p.Reviews)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (pokemon == null)
            {
                return false; 
            }

            // Removendo todas as relações
            _context.PokemonOwners.RemoveRange(pokemon.PokemonOwners);
            _context.PokemonCategories.RemoveRange(pokemon.PokemonCategories);
            _context.Reviews.RemoveRange(pokemon.Reviews);

            // Removendo o Pokémon
            _context.Pokemon.Remove(pokemon);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
