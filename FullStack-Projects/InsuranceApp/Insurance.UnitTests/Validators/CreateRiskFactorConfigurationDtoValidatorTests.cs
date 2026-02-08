using FluentAssertions;
using Insurance.Application.DTOs.RiskFactors;
using Insurance.Application.Validators.RiskFactors;
using Xunit;

namespace Insurance.UnitTests.Validators;

public class CreateRiskFactorConfigurationDtoValidatorTests
{
    private readonly CreateRiskFactorConfigurationDtoValidator _validator = new();

    [Fact]
    public void Should_Fail_When_Level_Is_Empty()
    {
        var dto = new CreateRiskFactorConfigurationDto
        {
            Level = "",
            ReferenceId = Guid.NewGuid(),
            AdjustmentPercentage = 5
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Fail_When_AdjustmentPercentage_Is_Zero()
    {
        var dto = new CreateRiskFactorConfigurationDto
        {
            Level = "City",
            ReferenceId = Guid.NewGuid(),
            AdjustmentPercentage = 0
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Data_Is_Valid()
    {
        var dto = new CreateRiskFactorConfigurationDto
        {
            Level = "City",
            ReferenceId = Guid.NewGuid(),
            AdjustmentPercentage = 5
        };

        _validator.Validate(dto).IsValid.Should().BeTrue();
    }
}
