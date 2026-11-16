using FluentAssertions;
using Insurance.Application.DTOs.Currencies;
using Insurance.Application.Validators.Currencies;
using Xunit;

namespace Insurance.UnitTests.Validators;

public class CreateCurrencyDtoValidatorTests
{
    private readonly CreateCurrencyDtoValidator _validator = new();

    [Fact]
    public void Should_Fail_When_Code_Is_Too_Short()
    {
        var dto = new CreateCurrencyDto
        {
            Code = "R",
            Name = "RON",
            ExchangeRateToBase = 1
        };

        _validator.Validate(dto).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Data_Is_Valid()
    {
        var dto = new CreateCurrencyDto
        {
            Code = "RON",
            Name = "Romanian Leu",
            ExchangeRateToBase = 1
        };

        _validator.Validate(dto).IsValid.Should().BeTrue();
    }
}
