using FluentValidation;
using Insurance.Application.DTOs.Clients;
using Insurance.Domain.Enums;

namespace Insurance.Application.Validators.Clients;

public class CreateClientDtoValidator:AbstractValidator<CreateClientDto>
{
    public CreateClientDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Invalid format for email");

        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required.");

        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");

        RuleFor(x => x.ClientType).IsInEnum().WithMessage("Client type is invalid.");

        RuleFor(x => x.Cnp).NotEmpty().When(x => x.ClientType == ClientType.Individual).WithMessage("CNP is required for individual clients.");

        RuleFor(x => x.Cui).NotEmpty().When(x => x.ClientType == ClientType.Company).WithMessage("CUI is required for company clients.");
    }
}

