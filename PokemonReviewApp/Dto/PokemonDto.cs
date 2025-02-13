namespace PokemonReviewApp.Dto
{
    public class PokemonInputModel
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int CategoryId { get; set; }
    }

    public class PokemonOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
