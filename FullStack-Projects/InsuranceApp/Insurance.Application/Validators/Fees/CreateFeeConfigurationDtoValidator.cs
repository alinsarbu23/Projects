using FluentValidation;
using Insurance.Application.DTOs.Fees;

namespace Insurance.Application.Validators.Fees;

public class CreateFeeConfigurationDtoValidator : AbstractValidator<CreateFeeConfigurationDto>
{
    public CreateFeeConfigurationDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .MaximumLength(200)
            .Must(name => !name.Any(char.IsDigit))
            .WithMessage("Name must not contain digits.");

        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .MaximumLength(50);

        RuleFor(x => x.Percentage)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Percentage must be between 0 and 100.");

        RuleFor(x => x.EffectiveFrom)
            .NotEmpty();

        RuleFor(x => x)
            .Must(x =>
                x.EffectiveTo == null || x.EffectiveTo > x.EffectiveFrom)
            .WithMessage("EffectiveTo must be after EffectiveFrom.");
    }
}
