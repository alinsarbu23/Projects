using FluentAssertions;
using Insurance.Application.DTOs.Brokers;
using Insurance.Application.Validators.Brokers;
using Xunit;

namespace Insurance.UnitTests.Validators;

public class CreateBrokerDtoValidatorTests
{
    private readonly CreateBrokerDtoValidator _validator = new();

    [Fact]
    public void Should_Fail_When_Email_Invalid()
    {
        var dto = new CreateBrokerDto
        {
            BrokerCode = "BR-01",
            Name = "Broker",
            Email = "invalid",
            CommissionPercentage = 1
        };

        _validator.Validate(dto).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Data_Is_Valid()
    {
        var dto = new CreateBrokerDto
        {
            BrokerCode = "BR-01",
            Name = "Broker",
            Email = "broker@test.ro",
            CommissionPercentage = 1
        };

        _validator.Validate(dto).IsValid.Should().BeTrue();
    }
}
