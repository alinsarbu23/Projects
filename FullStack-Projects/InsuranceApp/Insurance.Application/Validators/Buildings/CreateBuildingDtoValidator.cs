using FluentValidation;
using Insurance.Application.DTOs.Buildings;

public class CreateBuildingDtoValidator : AbstractValidator<CreateBuildingDto>
{
    public CreateBuildingDtoValidator()
    {

        RuleFor(x => x.CityId).NotEmpty().WithMessage("City is required.");

        RuleFor(x => x.BuildingTypeId).NotEmpty().WithMessage("Building type is required.");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(200).WithMessage("Street name is too long.")
            .Must(s => s.Any(char.IsLetter))
            .WithMessage("Street must contain at least one letter.");

        RuleFor(x => x.StreetNumber)
            .NotEmpty().WithMessage("Street number is required.")
            .MaximumLength(20).WithMessage("Street number is too long.")
            .Must(n => n.Any(char.IsDigit))
            .WithMessage("Street number must contain at least one digit.");

        RuleFor(x => x.ConstructionYear)
            .InclusiveBetween(1800, DateTime.UtcNow.Year)
            .When(x => x.ConstructionYear.HasValue)
            .WithMessage($"ConstructionYear must be between 1800 and {DateTime.UtcNow.Year}.");

        RuleFor(x => x.NumberOfFloors)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(150)
            .When(x => x.NumberOfFloors.HasValue)
            .WithMessage("NumberOfFloors must be between 0 and 150.");

        RuleFor(x => x)
            .Must(x =>
                !x.NumberOfFloors.HasValue ||
                x.NumberOfFloors == 0 ||
                x.ConstructionYear.HasValue)
            .WithMessage("ConstructionYear is required when NumberOfFloors is greater than 0.");

        RuleFor(x => x.SurfaceArea)
            .GreaterThan(5)
            .WithMessage("SurfaceArea must be greater than 5 square meters.")
            .LessThanOrEqualTo(50_000)
            .WithMessage("SurfaceArea exceeds realistic limits.");

        RuleFor(x => x.InsuredValue)
            .GreaterThan(1_000)
            .WithMessage("InsuredValue must be greater than 1,000.")
            .LessThanOrEqualTo(500_000_000)
            .WithMessage("InsuredValue exceeds realistic limits.");

      
    }
}
