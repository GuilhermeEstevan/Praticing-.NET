using FluentValidation;
using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Helper.Validators
{
    public class CountryValidator : AbstractValidator<CountryInputModel>
    {
        public CountryValidator() 
        {
            RuleFor(country => country.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Matches("^[a-zA-Z ]+$").WithMessage("Name should contain only letters and spaces.")
                .Must(name => !string.IsNullOrWhiteSpace(name?.Trim())).WithMessage("Name cannot be only spaces.");
        }
    }
}


