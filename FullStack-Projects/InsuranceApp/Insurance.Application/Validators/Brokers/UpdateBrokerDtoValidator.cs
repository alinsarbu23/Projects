using FluentValidation;
using Insurance.Application.DTOs.Brokers;

namespace Insurance.Application.Validators.Brokers;

public class UpdateBrokerDtoValidator : AbstractValidator<UpdateBrokerDto>
{
    public UpdateBrokerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .Must(name => !name.Any(char.IsDigit))
            .WithMessage("Name must not contain digits.");

        RuleFor(x => x.Phone)
            .Must(phone => phone != null && phone.All(char.IsDigit))
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("Phone must contain digits only.")
            .Length(10, 15)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("Phone must be between 10 and 15 digits.");

        RuleFor(x => x.CommissionPercentage)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(50)
            .WithMessage("Commission must be between 0 and 50 percent.");
    }
}
