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

        // Método assíncrono para recuperar Pokémons
        public async Task<ICollection<Pokemon>> GetPokemons()
        {
            // Usando o método assíncrono ToListAsync() para obter os dados de forma não bloqueante
            return await _context.Pokemon.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<Pokemon> GetPokemonById(int id)
        {
            var pokemon = await _context.Pokemon
                                         .Where(p => p.Id == id)
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


    }
}
