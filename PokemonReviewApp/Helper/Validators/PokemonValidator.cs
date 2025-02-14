using FluentValidation;
using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Helper.Validators
{
    public class PokemonValidator : AbstractValidator<PokemonInputModel>
    {
        public PokemonValidator()
        {
            // Validação para o nome: deve conter apenas letras e espaços
            RuleFor(pokemon => pokemon.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Matches("^[a-zA-Z ]+$").WithMessage("Name must contain only letters and spaces.")
                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

            // Validação para a data de nascimento: não pode ser maior que a data atual
            RuleFor(pokemon => pokemon.BirthDate)
                .NotEmpty().WithMessage("BirthDate is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("BirthDate cannot be in the future.");
        }
    }

    public class PokemonUpdateValidator : AbstractValidator<PokemonUpdateModel>
    {
        public PokemonUpdateValidator()
        {
            RuleFor(pokemon => pokemon.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Matches("^[a-zA-Z ]+$").WithMessage("Name should contain only letters and spaces.");

            RuleFor(pokemon => pokemon.CategoryIds)
                .NotNull().WithMessage("CategoryIds cannot be null.")
                .Must(list => list.All(id => id > 0)).WithMessage("All CategoryIds must be valid positive numbers.");
        }
    }
}
