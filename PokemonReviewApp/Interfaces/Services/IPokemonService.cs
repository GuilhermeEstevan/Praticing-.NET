using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Interfaces.Services
{
    public interface IPokemonService
    {
        Task<ICollection<PokemonOutputModel>> GetPokemons();
        Task<PokemonOutputModel> GetPokemonById(int id);
        Task<decimal> GetPokemonRating(int pokemonId);
        Task<bool> PokemonExist(int id);
        Task<PokemonOutputModel> GetPokemonByName(string name);
        Task<PokemonOutputModel> CreatePokemon(PokemonInputModel pokemonInputModel);
        Task<PokemonOutputModel> UpdatePokemon(int id, PokemonUpdateModel pokemonInputModel);
        Task<bool> DeletePokemon(int id);
    }
}
