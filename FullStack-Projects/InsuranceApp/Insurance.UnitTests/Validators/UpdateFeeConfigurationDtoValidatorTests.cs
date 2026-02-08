using FluentAssertions;
using Insurance.Application.DTOs.Fees;
using Insurance.Application.Validators.Fees;
using Xunit;

namespace Insurance.UnitTests.Validators;

public class UpdateFeeConfigurationDtoValidatorTests
{
    private readonly UpdateFeeConfigurationDtoValidator _validator = new();

    [Fact]
    public void Should_Fail_When_Percentage_Is_Zero()
    {
        var dto = new UpdateFeeConfigurationDto { Percentage = 0 };

        _validator.Validate(dto).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Percentage_Positive()
    {
        var dto = new UpdateFeeConfigurationDto { Percentage = 5 };

        _validator.Validate(dto).IsValid.Should().BeTrue();
    }
}
