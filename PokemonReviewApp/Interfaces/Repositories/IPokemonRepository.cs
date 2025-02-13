using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces.Repositories
{
    public interface IPokemonRepository
    {
        Task<ICollection<Pokemon>> GetPokemons();
        Task<Pokemon> GetPokemonById(int id);
        Task<Pokemon> GetPokemonByName(string name);
        Task<decimal> GetPokemonRating(int pokemonId);
        Task<bool> PokemonExist(int id);
        Task<bool> PokemonNameAlreadyExists(string name);
        Task<Pokemon> CreatePokemon(Pokemon pokemon);


    }
}

