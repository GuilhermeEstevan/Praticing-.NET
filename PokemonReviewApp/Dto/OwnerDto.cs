using PokemonReviewApp.Models;

namespace PokemonReviewApp.Dto
{
    public class OwnerInputModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
        public int CountryId { get; set; }
    }
    public class OwnerOutputModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
        public CountryOutputModel Country { get; set; }
    }
}
