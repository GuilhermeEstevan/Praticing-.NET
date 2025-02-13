using PokemonReviewApp.Dto;


namespace PokemonReviewApp.Interfaces.Services
{
    public interface IReviewService
    {
        Task<ICollection<ReviewOutputModel>> GetReviews();
        Task<ReviewOutputModel> GetReview(int id);
        Task<ICollection<ReviewOutputModel>> GetReviewsByPokemon(int pokemonId);
        Task<bool> ReviewExists(int pokemonId);
        Task<ReviewSummaryWithReviewerNameOutputModel> CreateReview(ReviewInputModel reviewInputModel);
    }
}
