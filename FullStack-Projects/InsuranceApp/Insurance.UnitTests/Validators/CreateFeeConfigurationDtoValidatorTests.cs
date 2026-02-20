using FluentAssertions;
using Insurance.Application.DTOs.Fees;
using Insurance.Application.Validators.Fees;
using Insurance.Domain.Enums;
using Xunit;

namespace Insurance.UnitTests.Validators;

public class CreateFeeConfigurationDtoValidatorTests
{
    private readonly CreateFeeConfigurationDtoValidator _validator = new();

    [Fact]
    public void Should_Fail_When_Percentage_Is_Zero()
    {
        var dto = new CreateFeeConfigurationDto
        {
            Name = "Fee",
            Type = (int)FeeType.AdminFee,
            AppliesToRisk = (int)BuildingRiskType.None,
            Percentage = 0,
            EffectiveFrom = DateTime.UtcNow
        };

        _validator.Validate(dto).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Fail_When_EffectiveTo_Before_EffectiveFrom()
    {
        var dto = new CreateFeeConfigurationDto
        {
            Name = "Fee",
            Type = (int)FeeType.AdminFee,
            AppliesToRisk = (int)BuildingRiskType.None,
            Percentage = 5,
            EffectiveFrom = DateTime.UtcNow,
            EffectiveTo = DateTime.UtcNow.AddDays(-1)
        };

        _validator.Validate(dto).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Data_Is_Valid()
    {
        var dto = new CreateFeeConfigurationDto
        {
            Name = "Fee",
            Type = (int)FeeType.AdminFee,
            AppliesToRisk = (int)BuildingRiskType.None,
            Percentage = 5,
            EffectiveFrom = DateTime.UtcNow
        };

        _validator.Validate(dto).IsValid.Should().BeTrue();
    }
}
