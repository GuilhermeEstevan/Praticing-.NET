namespace PokemonReviewApp.Interfaces.Repositories
{
    public interface IOwnerRepository
    {
        Task<bool> OwnerExists(int ownerId);
    }
}
