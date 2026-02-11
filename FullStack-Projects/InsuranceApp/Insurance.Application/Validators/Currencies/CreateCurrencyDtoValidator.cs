using FluentValidation;
using Insurance.Application.DTOs.Currencies;

namespace Insurance.Application.Validators.Currencies;

public class CreateCurrencyDtoValidator : AbstractValidator<CreateCurrencyDto>
{
    public CreateCurrencyDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .Length(3, 10)
            .Must(code => code.All(char.IsLetter))
            .WithMessage("Currency code must contain only letters.");

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
    }
}
