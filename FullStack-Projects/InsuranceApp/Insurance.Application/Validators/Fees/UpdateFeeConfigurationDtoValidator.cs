using FluentValidation;
using Insurance.Application.DTOs.Fees;

namespace Insurance.Application.Validators.Fees;

public class UpdateFeeConfigurationDtoValidator : AbstractValidator<UpdateFeeConfigurationDto>
{
    public UpdateFeeConfigurationDtoValidator()
    {
        RuleFor(x => x.Percentage)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Percentage must be between 0 and 100.");

        RuleFor(x => x.IsActive)
            .Must(v => v == 0 || v == 1)
            .WithMessage("IsActive must be 0 or 1.");
    }
}
