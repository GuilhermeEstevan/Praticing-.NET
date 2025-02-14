using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces.Repositories
{
    public interface ICountryRepository
    {
        Task<ICollection<Country>> GetCountries();
        Task<Country> GetCountry(int id);
        Task<Country> GetCountryByOwner(int ownerId);
        Task<ICollection<Owner>> GetOwnersByCountry(int countryId);
        Task<bool> CountryExists(int id);
        Task<Country> CreateCountry(Country country);
        Task<bool> CountryNameAlreadyExists(string name);
        Task<Country> UpdateCountry(Country country);
    }
}
