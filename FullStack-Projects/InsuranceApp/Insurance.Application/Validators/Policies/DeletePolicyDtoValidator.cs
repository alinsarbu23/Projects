using FluentValidation;
using Insurance.Application.DTOs.Policies;

namespace Insurance.Application.Validators.Policies;

public class DeletePolicyDtoValidator : AbstractValidator<DeletePolicyDto>
{
    public DeletePolicyDtoValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .MaximumLength(255);
    }
}
