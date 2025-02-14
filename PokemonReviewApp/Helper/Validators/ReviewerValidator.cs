using FluentValidation;
using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Helper.Validators
{
    public class ReviewerValidator : AbstractValidator<ReviewerInputModel>
    {
        public ReviewerValidator()
        {
            RuleFor(owner => owner.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Matches("^[a-zA-Z]+$").WithMessage("First name should contain only letters.");

            RuleFor(owner => owner.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Matches("^[a-zA-Z]+$").WithMessage("Last name should contain only letters.");


        }
    }
}