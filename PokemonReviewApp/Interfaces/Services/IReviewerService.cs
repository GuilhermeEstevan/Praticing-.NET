using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Interfaces.Services
{
    public interface IReviewerService
    {
        Task<ICollection<ReviewerOutputModel>> GetReviewers();
        Task<ReviewerOutputModel> GetReviewerById(int reviewerId);
        Task<ICollection<ReviewSummaryWithReviewerNameOutputModel>> GetReviewsByReviewer(int reviewerId);
        Task<bool> ReviewerExists(int reviewerId);
    }
}
