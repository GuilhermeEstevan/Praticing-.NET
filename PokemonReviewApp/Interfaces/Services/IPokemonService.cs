using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Interfaces.Services
{
    public interface IPokemonService
    {
        Task<ICollection<PokemonDto>> GetPokemons();
        Task<PokemonDto> GetPokemonById(int id);
        Task<decimal> GetPokemonRating(int pokemonId);
        Task<bool> PokemonExist(int id);
        Task<PokemonDto> GetPokemonByName(string name);
    }
}
