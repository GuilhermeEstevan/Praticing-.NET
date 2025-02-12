﻿using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces.Repositories
{
    public interface IReviewerRepository
    {
    
        Task<ICollection<Reviewer>> GetReviewers();
        Task<Reviewer> GetReviewerById(int reviewerId);
        Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId);
        Task<bool> ReviewerExists(int reviewerId);
    }
}
