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
    public class ReviewerUpdateValidator : AbstractValidator<ReviewerUpdateModel>
    {
        public ReviewerUpdateValidator()
        {
            RuleFor(reviewer => reviewer.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .Matches("^[a-zA-Z ]+$").WithMessage("First Name should contain only letters and spaces.");

            RuleFor(reviewer => reviewer.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .Matches("^[a-zA-Z ]+$").WithMessage("Last Name should contain only letters and spaces.");
        }
    }
}