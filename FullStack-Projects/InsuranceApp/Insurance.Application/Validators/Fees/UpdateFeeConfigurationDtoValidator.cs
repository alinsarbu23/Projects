using FluentValidation;
using Insurance.Application.DTOs.Fees;

namespace Insurance.Application.Validators.Fees;

public class UpdateFeeConfigurationDtoValidator
    : AbstractValidator<UpdateFeeConfigurationDto>
{
    public UpdateFeeConfigurationDtoValidator()
    {
        RuleFor(x => x.Percentage)
            .GreaterThan(0);
    }
}
