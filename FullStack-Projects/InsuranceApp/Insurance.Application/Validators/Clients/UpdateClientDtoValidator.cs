using FluentValidation;
using Insurance.Application.DTOs.Clients;

namespace Insurance.Application.Validators.Clients;

public class UpdateClientDtoValidator : AbstractValidator<UpdateClientDto>
{
    public UpdateClientDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name is too long.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Phone is required.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Address is required.");
    }
}
