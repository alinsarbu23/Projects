using FluentValidation;
using Insurance.Application.DTOs.Identity;

namespace Insurance.Application.Validators.Identity;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.Password).NotEmpty();
    }
}
