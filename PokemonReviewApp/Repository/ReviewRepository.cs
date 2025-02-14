using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            this._context = context;
        }
        public async Task<ICollection<Review>> GetReviews()
        {
            return await _context.Reviews
                                    .OrderByDescending(review => review.Id)
                                    .Include(r => r.Reviewer)
                                    .Include(r => r.Pokemon)
                                    .ToListAsync();

        }
        public async Task<Review> GetReview(int id)
        {
            return await _context.Reviews
                                    .Include(r => r.Reviewer)
                                    .Include(r => r.Pokemon)
                                    .Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Review>> GetReviewsByPokemon(int pokemonId)
        {
            return await _context.Reviews
                                    .Include(r => r.Reviewer)
                                    .Include(r => r.Pokemon)
                                    .Where(r => r.Pokemon.Id == pokemonId)
                                    .ToListAsync();
        }

        public async Task<bool> ReviewExists(int pokemonId)
        {
            return await _context.Reviews.AnyAsync();  
        }

        public async Task<Review> CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review> UpdateReview(Review review)
        {
            _context.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteReview(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);

            if (review == null)
            {
                return false;
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
