﻿using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces.Repositories;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context) 
        {
            this._context = context;
        }
        public async Task<bool> CategoryExists(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId );
        }

        public async Task<ICollection<Category>> GetCategories()
        {
            return await _context.Categories.OrderByDescending(c => c.Name).ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.Where(c => c.Id == id).FirstAsync();
        }

        public async Task<ICollection<Pokemon>> GetPokemonsByCategory(int categoryId)
        {
            return await _context.Pokemon
                                    .Where(p => p.PokemonCategories
                                    .Any(pc => pc.CategoryId == categoryId))
                                    .ToListAsync();
        }

    }
}
