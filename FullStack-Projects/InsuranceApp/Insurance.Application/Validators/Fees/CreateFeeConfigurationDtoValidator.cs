using FluentValidation;
using Insurance.Application.DTOs.Fees;
using Insurance.Domain.Enums;

namespace Insurance.Application.Validators.Fees;

public class CreateFeeConfigurationDtoValidator : AbstractValidator<CreateFeeConfigurationDto>
{
    public CreateFeeConfigurationDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .Must(name => !name.Any(char.IsDigit))
            .WithMessage("Name must not contain digits.");

        RuleFor(x => x.Type)
            .Must(type => Enum.IsDefined(typeof(FeeType), type))
            .WithMessage("Invalid fee type.");

        RuleFor(x => x.AppliesToRisk)
            .Must(risk => Enum.IsDefined(typeof(BuildingRiskType), risk))
            .WithMessage("Invalid risk type.");

        RuleFor(x => x.Percentage)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Percentage must be between 0 and 100.");

        RuleFor(x => x)
            .Must(x => x.EffectiveTo == null || x.EffectiveTo > x.EffectiveFrom)
            .WithMessage("EffectiveTo must be after EffectiveFrom.");
    }
}
