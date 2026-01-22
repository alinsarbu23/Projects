using FluentValidation;
using Insurance.Application.DTOs.Buildings;

public class CreateBuildingDtoValidator : AbstractValidator<CreateBuildingDto>
{
    public CreateBuildingDtoValidator()
    {
        RuleFor(x => x.CityId).NotEmpty();

        RuleFor(x => x.BuildingTypeId).NotEmpty();

        RuleFor(x => x.Street).NotEmpty();

        RuleFor(x => x.StreetNumber).NotEmpty();

        RuleFor(x => x.SurfaceArea).GreaterThan(0);

        RuleFor(x => x.InsuredValue).GreaterThan(0);
    }
}
