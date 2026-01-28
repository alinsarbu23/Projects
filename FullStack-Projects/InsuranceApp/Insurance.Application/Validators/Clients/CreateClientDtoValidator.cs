using FluentValidation;
using Insurance.Application.DTOs.Clients;
using Insurance.Domain.Enums;

namespace Insurance.Application.Validators.Clients;

public class CreateClientDtoValidator : AbstractValidator<CreateClientDto>
{
    public CreateClientDtoValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.");

        RuleFor(x => x.ClientType)
            .IsInEnum().WithMessage("Client type is invalid.");

        RuleFor(x => x.Cnp)
            .NotEmpty()
            .When(x => x.ClientType == ClientType.Individual)
            .WithMessage("CNP is required for individual clients.");

        RuleFor(x => x.Cnp)
            .Length(13)
            .When(x => x.ClientType == ClientType.Individual && !string.IsNullOrEmpty(x.Cnp))
            .WithMessage("CNP must contain exactly 13 digits.");

        RuleFor(x => x.Cnp)
            .Must(cnp => cnp!.All(char.IsDigit))
            .When(x => x.ClientType == ClientType.Individual && !string.IsNullOrEmpty(x.Cnp))
            .WithMessage("CNP must contain only digits.");

        RuleFor(x => x.Cui)
            .NotEmpty()
            .When(x => x.ClientType == ClientType.Company)
            .WithMessage("CUI is required for company clients.");

        RuleFor(x => x)
            .Must(x =>
                !(!string.IsNullOrWhiteSpace(x.Cnp) &&
                   !string.IsNullOrWhiteSpace(x.Cui)))
            .WithMessage("CNP and CUI cannot both be provided.");

        RuleFor(x => x.Cnp)
            .Empty()
            .When(x => x.ClientType == ClientType.Company)
            .WithMessage("CNP must not be provided for company clients.");

        RuleFor(x => x.Cui)
            .Empty()
            .When(x => x.ClientType == ClientType.Individual)
            .WithMessage("CUI must not be provided for individual clients.");
    }
}
