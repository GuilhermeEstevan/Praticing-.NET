using FluentValidation;
using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Helper.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryInputModel>
    {
        public CategoryValidator()
        {
            RuleFor(category => category.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Matches("^[a-zA-Z]+$").WithMessage("Name should contain only letters.");
        }
    }
}