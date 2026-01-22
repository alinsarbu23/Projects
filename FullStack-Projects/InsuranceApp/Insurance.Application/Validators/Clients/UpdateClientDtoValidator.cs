using FluentValidation;
using Insurance.Application.DTOs.Clients;

public class UpdateClientDtoValidator : AbstractValidator<UpdateClientDto>
{
    public UpdateClientDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.Phone).NotEmpty();

        RuleFor(x => x.Address).NotEmpty();
    }
}
