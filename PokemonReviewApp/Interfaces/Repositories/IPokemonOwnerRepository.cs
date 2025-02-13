namespace PokemonReviewApp.Interfaces.Repositories
{
    public interface IPokemonOwnerRepository
    {
        Task AddPokemonOwner(int pokemonId, int ownerId);
    }
}
