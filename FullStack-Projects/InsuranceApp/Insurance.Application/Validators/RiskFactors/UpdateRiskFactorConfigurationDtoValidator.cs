using FluentValidation;
using Insurance.Application.DTOs.RiskFactors;

namespace Insurance.Application.Validators.RiskFactors;

public class UpdateRiskFactorConfigurationDtoValidator : AbstractValidator<UpdateRiskFactorConfigurationDto>
{
    public UpdateRiskFactorConfigurationDtoValidator()
    {
        RuleFor(x => x.AdjustmentPercentage)
            .NotEqual(0);
    }
}
