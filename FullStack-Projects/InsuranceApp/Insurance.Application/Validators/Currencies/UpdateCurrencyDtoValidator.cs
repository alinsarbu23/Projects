using FluentValidation;
using Insurance.Application.DTOs.Currencies;

namespace Insurance.Application.Validators.Currencies;

public class UpdateCurrencyDtoValidator : AbstractValidator<UpdateCurrencyDto>
{
    public UpdateCurrencyDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .MaximumLength(50)
            .Must(name => !name.Any(char.IsDigit))
            .WithMessage("Name must not contain digits.");

        RuleFor(x => x.ExchangeRateToBase)
            .GreaterThan(0)
            .LessThanOrEqualTo(1000)
            .WithMessage("Exchange rate must be greater than 0 and realistic.");

        RuleFor(x => x.IsActive)
            .Must(v => v == 0 || v == 1)
            .WithMessage("IsActive must be 0 or 1.");
    }
}
