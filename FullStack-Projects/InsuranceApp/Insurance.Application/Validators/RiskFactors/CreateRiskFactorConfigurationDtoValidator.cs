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
            .MaximumLength(50);

        RuleFor(x => x.ReferenceId)
            .NotEmpty();

        RuleFor(x => x.AdjustmentPercentage)
            .NotEqual(0);
    }
}
