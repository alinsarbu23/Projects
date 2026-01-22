using FluentValidation;
using Insurance.Application.DTOs.Buildings;

public class UpdateBuildingDtoValidator: AbstractValidator<UpdateBuildingDto>
{
    public UpdateBuildingDtoValidator()
    {
        RuleFor(x => x.Street).NotEmpty();

        RuleFor(x => x.StreetNumber).NotEmpty();

        RuleFor(x => x.SurfaceArea).GreaterThan(0);

        RuleFor(x => x.InsuredValue).GreaterThan(0);
    }
}
