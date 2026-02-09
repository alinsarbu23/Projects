using FluentValidation;
using Insurance.Application.DTOs.Brokers;

namespace Insurance.Application.Validators.Brokers;

public class UpdateBrokerDtoValidator : AbstractValidator<UpdateBrokerDto>
{
    public UpdateBrokerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .MaximumLength(200);

        RuleFor(x => x.Phone)
            .Must(x => string.IsNullOrWhiteSpace(x) || !string.IsNullOrWhiteSpace(x))
            .When(x => x.Phone != null);

        RuleFor(x => x.CommissionPercentage)
            .GreaterThanOrEqualTo(0);
    }
}
