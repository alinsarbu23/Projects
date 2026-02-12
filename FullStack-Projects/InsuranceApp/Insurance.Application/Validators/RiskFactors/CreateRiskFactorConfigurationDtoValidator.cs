using FluentValidation;
using Insurance.Application.DTOs.RiskFactors;
using Insurance.Domain.Enums;

namespace Insurance.Application.Validators.RiskFactors;

public class CreateRiskFactorConfigurationDtoValidator : AbstractValidator<CreateRiskFactorConfigurationDto>
{
    public CreateRiskFactorConfigurationDtoValidator()
    {
        RuleFor(x => x.Level)
            .Must(level => Enum.IsDefined(typeof(RiskLevel), level))
            .WithMessage("Invalid risk level.");

        RuleFor(x => x.ReferenceId)
            .NotEmpty();

        RuleFor(x => x.AdjustmentPercentage)
            .NotEqual(0)
            .GreaterThanOrEqualTo(-100)
            .LessThanOrEqualTo(100)
            .WithMessage("AdjustmentPercentage must be between -100 and 100 and cannot be 0.");
    }
}
