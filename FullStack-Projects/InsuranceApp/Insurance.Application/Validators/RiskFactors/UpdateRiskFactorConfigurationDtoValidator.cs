using FluentValidation;
using Insurance.Application.DTOs.RiskFactors;

namespace Insurance.Application.Validators.RiskFactors;

public class UpdateRiskFactorConfigurationDtoValidator : AbstractValidator<UpdateRiskFactorConfigurationDto>
{
    public UpdateRiskFactorConfigurationDtoValidator()
    {
        RuleFor(x => x.AdjustmentPercentage)
            .NotEqual(0);

        RuleFor(x => x.IsActive)
            .Must(v => v == 0 || v == 1)
            .WithMessage("IsActive must be 0 or 1.");
    }
}
