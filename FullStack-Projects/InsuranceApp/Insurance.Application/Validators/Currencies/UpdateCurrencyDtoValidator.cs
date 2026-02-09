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
            .MaximumLength(50);

        RuleFor(x => x.ExchangeRateToBase)
            .GreaterThan(0);

        RuleFor(x => x.IsActive)
            .Must(v => v == 0 || v == 1)
            .WithMessage("IsActive must be 0 or 1.");
    }
}
