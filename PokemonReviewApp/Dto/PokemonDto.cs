namespace PokemonReviewApp.Dto
{
    public class PokemonInputModel
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public List<int> CategoryIds { get; set; }
    }

    public class PokemonOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public List<CategoryOutputModel> Categories { get; set; }
    }

    public class PokemonUpdateModel
    {
        public string Name { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
