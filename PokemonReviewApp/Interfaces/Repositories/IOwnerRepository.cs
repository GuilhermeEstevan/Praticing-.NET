using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces.Repositories
{
    public interface IOwnerRepository
    {
        Task<ICollection<Owner>> GetOwners(int pageNumber, int pageSize);
        Task<Owner> GetOwner(int ownerId);
        Task<ICollection<Owner>> GetOwnersByPokemon(int pokemonId);
        Task<ICollection<Pokemon>> GetPokemonsByOwner(int ownerId);
        Task<bool> OwnerExists(int ownerId);
    }
}
