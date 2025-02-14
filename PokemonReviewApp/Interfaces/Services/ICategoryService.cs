using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<ICollection<CategoryOutputModel>> GetCategories();
        Task<CategoryOutputModel> GetCategoryById(int id);
        Task<ICollection<PokemonOutputModel>> GetPokemonsByCategory(int categoryId);
        Task<CategoryOutputModel> CreateCategory(CategoryInputModel categoryInputModel);
        Task<CategoryOutputModel> UpdateCategory(int id, CategoryInputModel categoryInputModel);
    }
}
