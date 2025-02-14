using FluentValidation;
using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Helper.Validators
{
    public class ReviewValidator : AbstractValidator<ReviewInputModel>
    {
        public ReviewValidator()
        {
            RuleFor(review => review.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot be longer than 50 characters.");
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Text is required.")
                .MaximumLength(500).WithMessage("Text cannot be longer than 500 characters.");
            RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
            RuleFor(x => x.ReviewerId).GreaterThan(0).WithMessage("ReviewerId must be a valid ID.");
            RuleFor(x => x.PokemonId).GreaterThan(0).WithMessage("PokemonId must be a valid ID.");
        }
    }
}
