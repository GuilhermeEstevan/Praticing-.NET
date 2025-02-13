using FluentValidation;
using PokemonReviewApp.Dto;

public class OwnerValidator : AbstractValidator<OwnerInputModel>
{
    public OwnerValidator()
    {
        RuleFor(owner => owner.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Matches("^[a-zA-Z]+$").WithMessage("First name should contain only letters.");

        RuleFor(owner => owner.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Matches("^[a-zA-Z]+$").WithMessage("Last name should contain only letters.");

        RuleFor(owner => owner.Gym)
            .NotEmpty().WithMessage("Gym name is required.")
            .Matches("^[a-zA-Z0-9 ]+$").WithMessage("Gym name can only contain letters, numbers, and spaces.");

        RuleFor(owner => owner.CountryId)
            .GreaterThan(0).WithMessage("CountryId must be a valid ID.");
    }
}
