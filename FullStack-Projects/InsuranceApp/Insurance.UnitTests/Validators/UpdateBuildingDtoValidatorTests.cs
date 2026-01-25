using Xunit;
using FluentAssertions;
using Insurance.Application.DTOs.Buildings;

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
            SurfaceArea = surface,
            InsuredValue = 100
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
            SurfaceArea = 10,
            InsuredValue = value
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
            SurfaceArea = 10,
            InsuredValue = 100
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }
}
