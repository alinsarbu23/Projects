using FluentValidation;
using Insurance.Application.DTOs.Clients;

public class UpdateClientIdentifierDtoValidator : AbstractValidator<UpdateClientIdentifierDto>
{
    public UpdateClientIdentifierDtoValidator()
    {
        RuleFor(x => x.NewIdentifier)
            .NotEmpty()
            .WithMessage("Identifier is required.")
            .MaximumLength(20);

        RuleFor(x => x.NewIdentifier)
            .Must(id => id.All(char.IsLetterOrDigit))
            .WithMessage("Identifier must contain only letters or digits.");

        RuleFor(x => x.NewIdentifier)
            .Must(id =>
                (id.All(char.IsDigit) && id.Length == 13) ||
                (id.StartsWith("RO") && id.Substring(2).All(char.IsDigit)) ||
                (id.Length >= 2 && id.Length <= 15))
            .WithMessage("Invalid identifier format.");
    }
}
