using PokemonReviewApp.Models;

namespace PokemonReviewApp.Dto
{
    public class ReviewerInputModel
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
      
    }
    public class ReviewerOutputModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<ReviewOutputForReviewer> Reviews { get; set; }
    }

    public class ReviewerNameOutputModel
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class ReviewerUpdateModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
