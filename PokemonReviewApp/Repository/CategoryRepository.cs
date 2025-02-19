﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<Category> CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<ICollection<Category>> GetCategories()
        {
            return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
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

        public async Task<bool> CategoryNameAlreadyExists(string name)
        {
            return await _context.Categories
                                    .AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<ICollection<Category>> GetCategoriesByIds(List<int> categoryIds)
        {
            return await _context.Categories
                                 .Where(c => categoryIds.Contains(c.Id))
                                 .ToListAsync();
        }

        public async Task<bool> HasPokemons(int categoryId)
        {
            return await _context.PokemonCategories.AnyAsync(pc => pc.CategoryId == categoryId);
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);

            if (category == null)
            {
                return false; 
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
