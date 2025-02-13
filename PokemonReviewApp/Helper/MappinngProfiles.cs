using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Helper
{
    public class MappinngProfiles : Profile
    {
        public MappinngProfiles()
        {
            // Pokemon
            CreateMap<Pokemon, PokemonInputModel>();
            CreateMap<Pokemon, PokemonOutputModel>();
            // Category
            CreateMap<CategoryInputModel, Category>();
            CreateMap<Category, CategoryOutputModel>();
            // Country
            CreateMap<Country, CountryInputModel>();
            CreateMap<Country, CountryOutputModel>();
            // Owner
            CreateMap<Owner, OwnerInputModel>();
            CreateMap<Owner, OwnerOutputModel>();
            // Review
            CreateMap<Review, ReviewInputModel>();
            CreateMap<Review, ReviewOutputModel>();
            CreateMap<Review, ReviewOutputForReviewer>();
            CreateMap<Review, ReviewSummaryWithReviewerNameOutputModel>();
            // Reviewer
            CreateMap<Reviewer, ReviewerInputModel>();
            CreateMap<Reviewer, ReviewerOutputModel>();
            CreateMap<Reviewer, ReviewerNameOutputModel>();

        }
    }
}
