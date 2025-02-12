using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<bool> CountryExists(int id)
        {
            return await _context.Countries.AnyAsync(c => c.Id == id);
        }

        public async Task<ICollection<Country>> GetCountries()
        {
            return await _context.Countries.OrderByDescending(c => c.Name).ToListAsync();
        }

        public async Task<Country> GetCountry(int id)
        {
            return await _context.Countries.Where(c => c.Id == id).FirstAsync();
        }

        public async Task<Country> GetCountryByOwner(int ownerId)
        {
            return await _context.Owners
                                    .Where(o => o.Id == ownerId)
                                    .Select(o => o.Country)
                                    .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Owner>> GetOwnersByCountry(int countryId)
        {
            return await _context.Owners.Where(o => o.Country.Id == countryId).ToListAsync();
        }
    }
}
