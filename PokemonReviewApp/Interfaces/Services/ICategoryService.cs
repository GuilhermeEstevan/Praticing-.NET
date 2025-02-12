using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<ICollection<CategoryOutputModel>> GetCategories();
        Task<CategoryOutputModel> GetCategoryById(int id);
        Task<ICollection<PokemonOutputModel>> GetPokemonsByCategory(int categoryId);
    }
}
