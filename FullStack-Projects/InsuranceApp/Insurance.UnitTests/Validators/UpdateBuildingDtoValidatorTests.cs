using Xunit;
using FluentAssertions;
using Insurance.Application.DTOs.Buildings;
using Insurance.Application.Validators.Buildings;

namespace Insurance.UnitTests.Validators;

public class UpdateBuildingDtoValidatorTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ShouldFail_WhenSurfaceAreaInvalid(decimal surface)
    {
        var validator = new UpdateBuildingDtoValidator();

        var dto = new UpdateBuildingDto
        {
            Street = "Street",
            StreetNumber = "1",
            ConstructionYear = 2000,
            NumberOfFloors = 2,
            SurfaceArea = surface,
            InsuredValue = 100,
            FloodRisk = false,
            EarthquakeRisk = false
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }


    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void ShouldFail_WhenInsuredValueInvalid(decimal value)
    {
        var validator = new UpdateBuildingDtoValidator();

        var dto = new UpdateBuildingDto
        {
            Street = "Street",
            StreetNumber = "1",
            ConstructionYear = 2000,
            NumberOfFloors = 2,
            SurfaceArea = 10,
            InsuredValue = value,
            FloodRisk = false,
            EarthquakeRisk = false
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }


    [Fact]
    public void ShouldPass_WhenDataIsValid()
    {
        var validator = new UpdateBuildingDtoValidator();

        var dto = new UpdateBuildingDto
        {
            Street = "Street",
            StreetNumber = "1",
            ConstructionYear = 2000,
            NumberOfFloors = 2,
            SurfaceArea = 10,
            InsuredValue = 2000,  
            FloodRisk = false,
            EarthquakeRisk = false
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }


}
