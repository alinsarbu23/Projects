using FluentValidation;
using Insurance.Application.DTOs.Policies;

namespace Insurance.Application.Validators.Policies;

public class DeletePolicyDtoValidator : AbstractValidator<DeletePolicyDto>
{
    public DeletePolicyDtoValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Cancellation reason is required.")
            .MaximumLength(255)
            .WithMessage("Cancellation reason is too long.");
    }
}
