using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<ICollection<Review>> GetReviews();
        Task<Review> GetReview(int id);
        Task<ICollection<Review>> GetReviewsByPokemon(int pokemonId);
        Task<bool> ReviewExists(int pokemonId);
        Task<Review> CreateReview(Review review);
        Task<Review> UpdateReview(Review review);
        Task<bool> DeleteReview(int reviewId);
    }
}
