using FluentValidation;
using Insurance.Application.DTOs.Brokers;

namespace Insurance.Application.Validators.Brokers;

public class CreateBrokerDtoValidator : AbstractValidator<CreateBrokerDto>
{
    public CreateBrokerDtoValidator()
    {
        RuleFor(x => x.BrokerCode)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .Must(name => !name.Any(char.IsDigit))
            .WithMessage("Name must not contain digits.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(100)
            .Must(email => !email.Contains(" "))
            .WithMessage("Email must not contain spaces.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Must(phone => phone != null && phone.All(char.IsDigit))
            .WithMessage("Phone must contain digits only.")
            .Length(10, 15)
            .WithMessage("Phone must be between 10 and 15 digits.");

        RuleFor(x => x.CommissionPercentage)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(50)
            .WithMessage("Commission must be between 0 and 50 percent.");
    }
}
