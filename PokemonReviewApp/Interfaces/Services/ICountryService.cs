using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces.Services
{
    public interface ICountryService
    {
        Task<ICollection<CountryOutputModel>> GetCountries();
        Task<CountryOutputModel> GetCountryById(int countryId);
        Task<CountryOutputModel> GetCountryByOwner(int ownerId);
        Task<bool> CountryExists(int countryId);
        Task<CountryOutputModel> CreateCountry(CountryInputModel countryInputModel);
        Task<CountryOutputModel> UpdateCountry(int id, CountryInputModel countryInputModel);

        Task<bool> DeleteCountry(int countryId);
    }
}
