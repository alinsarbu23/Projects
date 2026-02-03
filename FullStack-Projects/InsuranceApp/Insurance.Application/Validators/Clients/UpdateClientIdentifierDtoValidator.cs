using FluentValidation;
using Insurance.Application.DTOs.Clients;

namespace Insurance.Application.Validators.Clients;
public class UpdateClientIdentifierDtoValidator : AbstractValidator<UpdateClientIdentifierDto>
{
    public UpdateClientIdentifierDtoValidator()
    {
        RuleFor(x => x.NewIdentifier)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("New identifier is required.")
            .MaximumLength(13)
            .WithMessage("New identifier must be at most 13 characters.");
    }
}
