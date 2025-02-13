using FluentValidation;
using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Helper.Validators
{
    public class PokemonValidator : AbstractValidator<PokemonInputModel>
    {
        public PokemonValidator()
        {
            // Validação para o nome: deve conter apenas letras (sem números)
            RuleFor(pokemon => pokemon.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Matches("^[a-zA-Z]+$").WithMessage("Name must not contain numbers or special characters.")
                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

            // Validação para a data de nascimento: não pode ser maior que a data atual
            RuleFor(pokemon => pokemon.BirthDate)
                .NotEmpty().WithMessage("BirthDate is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("BirthDate cannot be in the future.");
        }
    }
}