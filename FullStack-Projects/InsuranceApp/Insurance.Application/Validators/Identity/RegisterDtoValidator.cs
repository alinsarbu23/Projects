using FluentValidation;
using Insurance.Application.DTOs.Identity;

namespace Insurance.Application.Validators.Identity;
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}
