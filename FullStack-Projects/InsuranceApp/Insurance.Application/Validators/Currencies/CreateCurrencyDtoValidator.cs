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
            .Length(3, 10);

        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .MaximumLength(50);

        RuleFor(x => x.ExchangeRateToBase)
            .GreaterThan(0);
    }
}
