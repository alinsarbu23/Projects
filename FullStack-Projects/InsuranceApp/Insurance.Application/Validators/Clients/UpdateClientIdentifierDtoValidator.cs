using FluentValidation;
using Insurance.Application.DTOs.Clients;

public class UpdateClientIdentifierDtoValidator: AbstractValidator<UpdateClientIdentifierDto>
{
    public UpdateClientIdentifierDtoValidator()
    {
        RuleFor(x => x.NewIdentifier).NotEmpty().WithMessage("New identifier is required.");
    }
}
