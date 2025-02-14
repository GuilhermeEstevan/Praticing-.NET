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

        public async Task<Reviewer> CreateReviewer(Reviewer reviewer)
        {
            _context.Reviewers.Add(reviewer);
            await _context.SaveChangesAsync();
            return reviewer;
        }

        public async Task<bool> ReviewerNameAlreadyExists(string firstName, string lastName)
        {
            var reviewer = await _context.Reviewers
                                 .FirstOrDefaultAsync(r => r.FirstName
                                 .ToLower() == firstName.ToLower() && r.LastName
                                 .ToLower() == lastName.ToLower());


            // Se o reviewer for encontrado, retorna true, caso contrário, retorna false
            return reviewer != null;
        }

        public async Task<Reviewer> UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            await _context.SaveChangesAsync();
            return reviewer;
        }

        public async Task<bool> DeleteReviewer(int reviewerId)
        {
            var reviewer = await _context.Reviewers
                .Include(r => r.Reviews) // Inclui as reviews associadas
                .FirstOrDefaultAsync(r => r.Id == reviewerId);

            if (reviewer == null)
            {
                return false;
            }

            _context.Reviews.RemoveRange(reviewer.Reviews); // Remove todas as reviews associadas
            _context.Reviewers.Remove(reviewer);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
