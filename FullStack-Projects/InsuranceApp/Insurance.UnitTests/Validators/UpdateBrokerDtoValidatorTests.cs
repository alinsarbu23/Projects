using FluentAssertions;
using Insurance.Application.DTOs.Brokers;
using Insurance.Application.Validators.Brokers;
using Xunit;

namespace Insurance.UnitTests.Validators;

public class UpdateBrokerDtoValidatorTests
{
    private readonly UpdateBrokerDtoValidator _validator = new();

    [Fact]
    public void Should_Fail_When_Commission_Negative()
    {
        var dto = new UpdateBrokerDto
        {
            Name = "Broker",
            CommissionPercentage = -1
        };

        _validator.Validate(dto).IsValid.Should().BeFalse();
    }
}
