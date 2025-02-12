using PokemonReviewApp.Models;

namespace PokemonReviewApp.Dto
{
    public class CategoryInputModel
    {
        public string Name { get; set; }
    }

    public class CategoryOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
