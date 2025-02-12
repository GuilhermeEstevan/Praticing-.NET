using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Interfaces.Services
{
    public interface ICountryService
    {
        Task<ICollection<CountryOutputModel>> GetCountriesAsync();
        Task<CountryOutputModel> GetCountryByIdAsync(int countryId);
        Task<CountryOutputModel> GetCountryByOwnerAsync(int ownerId);
        Task<bool> CountryExistsAsync(int countryId);
    }
}
