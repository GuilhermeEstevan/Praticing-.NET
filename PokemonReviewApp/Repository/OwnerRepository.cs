using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<ICollection<Owner>> GetOwners(int pageNumber, int pageSize)
        {
            return await _context.Owners
                                 .Include(o => o.Country)
                                 .OrderByDescending(o => o.FirstName)
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public async Task<ICollection<Owner>> GetOwnersByPokemon(int pokemonId)
        {
            return await _context.PokemonOwners
                                    .Where(po => po.PokemonId == pokemonId)
                                    .Include(po => po.Owner)
                                    .ThenInclude(o => o.Country)
                                    .Select(po => po.Owner)
                                    .ToListAsync();
        }

        public async Task<Owner> GetOwner(int ownerId)
        {
            return await _context.Owners
                                    .Include(o => o.Country)
                                    .Where(o => o.Id == ownerId)
                                    .FirstOrDefaultAsync();
        }


        public async Task<ICollection<Pokemon>> GetPokemonsByOwner(int ownerId)
        {
            return await _context.PokemonOwners
                                    .Where(po => po.OwnerId == ownerId)
                                    .Select(po => po.Pokemon)
                                    .ToListAsync();
        }

        public async Task<bool> OwnerExists(int ownerId)
        {
            return await _context.Owners.AnyAsync(o => o.Id == ownerId);
        }

        public async Task<Owner> CreateOwner(Owner owner)
        {
            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();
            return owner;

        }

        public async Task<Owner> UpdateOwner(Owner owner)
        {
            _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
            return owner;
        }
    }
}
