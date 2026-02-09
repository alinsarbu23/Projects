using FluentValidation;
using Insurance.Application.DTOs.Policies;

namespace Insurance.Application.Validators.Policies;

public class CreatePolicyDtoValidator : AbstractValidator<CreatePolicyDto>
{
    public CreatePolicyDtoValidator()
    {
        RuleFor(x => x.BuildingId)
            .NotEmpty();

        RuleFor(x => x.CurrencyId)
            .NotEmpty();

        RuleFor(x => x.BasePremium)
            .GreaterThan(0);

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("StartDate must be before EndDate");

        RuleFor(x => x.EndDate)
            .GreaterThan(DateTime.UtcNow.Date)
            .WithMessage("EndDate must be in the future");
    }
}
