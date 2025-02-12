using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces.Repositories;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<bool> OwnerExists(int ownerId)
        {
            return await _context.Owners.AnyAsync(o => o.Id == ownerId);
        }
    }
}
