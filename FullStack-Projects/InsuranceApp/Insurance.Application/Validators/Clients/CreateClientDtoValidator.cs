using FluentValidation;
using Insurance.Application.DTOs.Clients;

namespace Insurance.Application.Validators.Clients;

public class CreateClientDtoValidator : AbstractValidator<CreateClientDto>
{
    private const int IndividualClientTypeId = 1;
    private const int CompanyClientTypeId = 2;

    public CreateClientDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Name is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .EmailAddress()
            .WithMessage("Email is required.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Phone is required.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Address is required.");


        RuleFor(x => x.ClientTypeId)
            .Must(id => id == IndividualClientTypeId || id == CompanyClientTypeId)
            .WithMessage("ClientTypeId is invalid.");


        RuleFor(x => x.Cnp)
            .NotEmpty()
            .When(x => x.ClientTypeId == IndividualClientTypeId)
            .WithMessage("CNP is required for individual clients.");

        RuleFor(x => x.Cnp)
            .Length(13)
            .When(x => x.ClientTypeId == IndividualClientTypeId && !string.IsNullOrEmpty(x.Cnp))
            .WithMessage("CNP must contain exactly 13 digits.");

        RuleFor(x => x.Cnp)
            .Must(cnp => cnp!.All(char.IsDigit))
            .When(x => x.ClientTypeId == IndividualClientTypeId && !string.IsNullOrEmpty(x.Cnp))
            .WithMessage("CNP must contain only digits.");


        RuleFor(x => x.Cui)
            .NotEmpty()
            .When(x => x.ClientTypeId == CompanyClientTypeId)
            .WithMessage("CUI is required for company clients.");

        RuleFor(x => x)
            .Must(x =>
                !(!string.IsNullOrWhiteSpace(x.Cnp)
                   && !string.IsNullOrWhiteSpace(x.Cui)))
            .WithMessage("CNP and CUI cannot both be provided.");

        RuleFor(x => x.Cnp)
            .Empty()
            .When(x => x.ClientTypeId == CompanyClientTypeId)
            .WithMessage("CNP must not be provided for company clients.");

        RuleFor(x => x.Cui)
            .Empty()
            .When(x => x.ClientTypeId == IndividualClientTypeId)
            .WithMessage("CUI must not be provided for individual clients.");
    }
}
