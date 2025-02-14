namespace PokemonReviewApp.Dto
{
    public class ReviewInputModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public int ReviewerId { get; set; }
        public int PokemonId { get; set; }
    }

    public class ReviewOutputModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }

        // Detalhes do Reviewer e Pokemon, com base no relacionamento.
        public ReviewerOutputModel Reviewer { get; set; }
        public PokemonOutputModel Pokemon { get; set; } 
    }

    // DTO para retornar as reviews de um reviewer (sem o Reviewer)
    public class ReviewOutputForReviewer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }

        public PokemonOutputModel Pokemon { get; set; } 
    }

    // DTO para retornar as reviews de um reviewer (Com apenas o nome do Reviewer)
    public class ReviewSummaryWithReviewerNameOutputModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public ReviewerNameOutputModel Reviewer { get; set; }
        public PokemonOutputModel Pokemon { get; set; }
    }

    public class ReviewUpdateModel
    {
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
