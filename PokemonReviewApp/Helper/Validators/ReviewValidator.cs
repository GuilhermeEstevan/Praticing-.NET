using FluentValidation;
using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Helper.Validators
{
    public class ReviewValidator : AbstractValidator<ReviewInputModel>
    {
        public ReviewValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.Text).NotEmpty().WithMessage("Text is required.");
            RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
            RuleFor(x => x.ReviewerId).GreaterThan(0).WithMessage("ReviewerId must be a valid ID.");
            RuleFor(x => x.PokemonId).GreaterThan(0).WithMessage("PokemonId must be a valid ID.");
        }
    }
}
