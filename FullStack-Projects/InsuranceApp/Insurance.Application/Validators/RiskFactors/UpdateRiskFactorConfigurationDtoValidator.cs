using FluentValidation;
using Insurance.Application.DTOs.RiskFactors;

namespace Insurance.Application.Validators.RiskFactors;

public class UpdateRiskFactorConfigurationDtoValidator : AbstractValidator<UpdateRiskFactorConfigurationDto>
{
    public UpdateRiskFactorConfigurationDtoValidator()
    {
        RuleFor(x => x.AdjustmentPercentage)
            .NotEqual(0)
            .GreaterThanOrEqualTo(-100)
            .LessThanOrEqualTo(100)
            .WithMessage("AdjustmentPercentage must be between -100 and 100 and cannot be 0.");

        RuleFor(x => x.IsActive)
            .Must(v => v == 0 || v == 1)
            .WithMessage("IsActive must be 0 or 1.");
    }
}
