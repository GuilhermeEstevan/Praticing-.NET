﻿using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces.Repositories
{
    public interface ICategoryRepository
    {

        Task<ICollection<Category>> GetCategories();
        Task<Category> GetCategoryById(int id);
        Task<ICollection<Pokemon>> GetPokemonsByCategory(int categoryId);
        Task<bool> CategoryExists(int categoryId);
        Task<Category> CreateCategory(Category category);
        Task<bool> CategoryNameAlreadyExists(string name);
        Task<Category> UpdateCategory(Category category);
        Task<ICollection<Category>> GetCategoriesByIds(List<int> categoryIds);
        Task<bool> HasPokemons(int categoryId);
        Task<bool> DeleteCategory(int categoryId);

    }
}
