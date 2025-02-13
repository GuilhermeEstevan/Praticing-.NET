namespace PokemonReviewApp.Interfaces.Services
{
    public interface IPokemonOwnerService
    {
        Task CapturePokemon(int pokemonId, int ownerId);
    }

}
