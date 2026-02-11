using FluentValidation;
using Insurance.Application.DTOs.RiskFactors;

namespace Insurance.Application.Validators.RiskFactors;

public class CreateRiskFactorConfigurationDtoValidator : AbstractValidator<CreateRiskFactorConfigurationDto>
{
    public CreateRiskFactorConfigurationDtoValidator()
    {
        RuleFor(x => x.Level)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .MaximumLength(50)
            .Must(level => !level.Any(char.IsDigit))
            .WithMessage("Level must not contain digits.");

        RuleFor(x => x.ReferenceId)
            .NotEmpty();

        RuleFor(x => x.AdjustmentPercentage)
            .NotEqual(0)
            .GreaterThanOrEqualTo(-100)
            .LessThanOrEqualTo(100)
            .WithMessage("AdjustmentPercentage must be between -100 and 100 and cannot be 0.");
    }
}
