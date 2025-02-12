using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;

        public ReviewerRepository(DataContext context)
        {
            this._context = context;
        }
        public async Task<ICollection<Reviewer>> GetReviewers()
        {
            return await _context.Reviewers
                                    .OrderByDescending(r => r.Id)
                                    .Include(r => r.Reviews)
                                    .ThenInclude(review => review.Pokemon)
                                    .ToListAsync();
        }
        public async Task<Reviewer> GetReviewerById(int reviewerId)
        {
            return await _context.Reviewers
                                    .Where(r => r.Id == reviewerId)
                                    .Include(r => r.Reviews)
                                    .ThenInclude(review => review.Pokemon)
                                    .FirstOrDefaultAsync();
        }


        public async Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId)
        {
            return await _context.Reviews
                                    .Where(r => r.Reviewer.Id == reviewerId)
                                    .Include(r => r.Reviewer)
                                    .Include(r => r.Pokemon)
                                    .ToListAsync();
        }

        public async Task<bool> ReviewerExists(int reviewerId)
        {
            return await _context.Reviewers.AnyAsync(r => r.Id == reviewerId);
        }
    }
}
