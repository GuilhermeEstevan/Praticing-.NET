﻿using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Interfaces.Services
{
    public interface IReviewerService
    {
        Task<ICollection<ReviewerOutputModel>> GetReviewers();
        Task<ReviewerOutputModel> GetReviewerById(int reviewerId);
        Task<ICollection<ReviewSummaryWithReviewerNameOutputModel>> GetReviewsByReviewer(int reviewerId);
        Task<bool> ReviewerExists(int reviewerId);
        Task<ReviewerOutputModel> CreateReviewer(ReviewerInputModel reviewerInputModel);
        Task<ReviewerOutputModel> UpdateReviewer(int id, ReviewerUpdateModel reviewerUpdateModel);
        Task<bool> DeleteReviewer(int reviewerId);
    }
}
