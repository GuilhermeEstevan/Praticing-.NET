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
            CreateMap<Category, CategoryInputModel>();
            CreateMap<Category, CategoryOutputModel>();
            // Country
            CreateMap<Country, CountryInputModel>();
            CreateMap<Country, CountryOutputModel>();
            // Owner
            CreateMap<Owner, OwnerInputModel>();
            CreateMap<Owner, OwnerOutputModel>();
    

        }
    }
}
