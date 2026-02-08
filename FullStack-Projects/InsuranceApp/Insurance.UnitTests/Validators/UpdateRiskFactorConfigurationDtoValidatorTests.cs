using FluentAssertions;
using Insurance.Application.DTOs.RiskFactors;
using Insurance.Application.Validators.RiskFactors;
using Xunit;

namespace Insurance.UnitTests.Validators;

public class UpdateRiskFactorConfigurationDtoValidatorTests
{
    private readonly UpdateRiskFactorConfigurationDtoValidator _validator = new();

    [Fact]
    public void Should_Fail_When_AdjustmentPercentage_Is_Zero()
    {
        var dto = new UpdateRiskFactorConfigurationDto
        {
            AdjustmentPercentage = 0,
            IsActive = true
        };

        _validator.Validate(dto).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_AdjustmentPercentage_Not_Zero()
    {
        var dto = new UpdateRiskFactorConfigurationDto
        {
            AdjustmentPercentage = -5,
            IsActive = true
        };

        _validator.Validate(dto).IsValid.Should().BeTrue();
    }
}
