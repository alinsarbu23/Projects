using FluentValidation;
using Insurance.Application.DTOs.Identity;

namespace Insurance.Application.Validators.Identity;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(100)
            .Must(email => !email.Contains(" "))
            .WithMessage("Email must not contain spaces.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Must(p => p.Any(char.IsLetter))
            .WithMessage("Password must contain at least one letter.")
            .Must(p => p.Any(char.IsDigit))
            .WithMessage("Password must contain at least one digit.")
            .Must(p => p.Any(ch => !char.IsLetterOrDigit(ch)))
            .WithMessage("Password must contain at least one special character.");
    }
}
