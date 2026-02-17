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
            .WithMessage("Name is too long.")
            .Must(name => !name.Any(char.IsDigit))
            .WithMessage("Name must not contain digits.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .Must(email => email != null && !email.Contains(" "))
            .WithMessage("Email must not contain spaces.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Phone is required.")
            .Must(phone => phone.All(char.IsDigit))
            .WithMessage("Phone must contain digits only.")
            .Length(10, 15)
            .WithMessage("Phone must be between 10 and 15 digits.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Address is required.")
            .MaximumLength(250)
            .WithMessage("Address is too long.");
    }
}
