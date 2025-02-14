using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Interfaces.Services
{
    public interface IOwnerService
    {
        Task<ICollection<OwnerOutputModel>> GetOwners(int pageNumber, int pageSize);
        Task<OwnerOutputModel> GetOwnerById(int ownerId);
        Task<ICollection<OwnerOutputModel>> GetOwnersByPokemon(int pokemonId);
        Task<ICollection<PokemonOutputModel>> GetPokemonsByOwner(int ownerId);
        Task<bool> OwnerExists(int ownerId);
        Task<OwnerOutputModel> CreateOwner(OwnerInputModel ownerInputModel);
        Task<OwnerOutputModel> UpdateOwner(int id, OwnerInputModel ownerInputModel);
    }
}
