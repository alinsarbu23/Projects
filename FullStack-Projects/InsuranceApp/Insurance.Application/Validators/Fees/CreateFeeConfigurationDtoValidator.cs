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
            .MaximumLength(200);

        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .MaximumLength(50);

        RuleFor(x => x.Percentage)
            .GreaterThan(0);

        RuleFor(x => x.EffectiveFrom)
            .NotEmpty();

        RuleFor(x => x)
            .Must(x =>
                x.EffectiveTo == null || x.EffectiveTo > x.EffectiveFrom)
            .WithMessage("EffectiveTo must be after EffectiveFrom.");
    }
}
