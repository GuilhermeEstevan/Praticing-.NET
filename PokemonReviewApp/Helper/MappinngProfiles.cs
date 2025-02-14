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
            CreateMap<PokemonInputModel, Pokemon>();
            CreateMap<Pokemon, PokemonOutputModel>()
               .ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
                   src.PokemonCategories.Select(pc => new CategoryOutputModel { Id = pc.Category.Id, Name = pc.Category.Name })
               ));
            CreateMap<PokemonUpdateModel, Pokemon>();
            // Category
            CreateMap<CategoryInputModel, Category>();
            CreateMap<Category, CategoryOutputModel>();
            // Country
            CreateMap<CountryInputModel, Country>();
            CreateMap<Country, CountryOutputModel>();
            // Owner
            CreateMap<OwnerInputModel, Owner>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => new Country { Id = src.CountryId }));
            CreateMap<Owner, OwnerOutputModel>();
            // Review
            CreateMap<ReviewInputModel, Review>();
            CreateMap<Review, ReviewOutputModel>();
            CreateMap<Review, ReviewOutputForReviewer>();
            CreateMap<Review, ReviewSummaryWithReviewerNameOutputModel>();
            // Reviewer
            CreateMap<ReviewerInputModel, Reviewer>();
            CreateMap<Reviewer, ReviewerOutputModel>();
            CreateMap<Reviewer, ReviewerNameOutputModel>();
            // PokemonOwner
            CreateMap<PokemonOwnerInputModel, PokemonOwner>();
        }
    }
}
