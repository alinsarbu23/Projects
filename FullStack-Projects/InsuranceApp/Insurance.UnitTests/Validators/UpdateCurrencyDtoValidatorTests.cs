using FluentAssertions;
using Insurance.Application.DTOs.Currencies;
using Insurance.Application.Validators.Currencies;
using Xunit;

namespace Insurance.UnitTests.Validators;

public class UpdateCurrencyDtoValidatorTests
{
    private readonly UpdateCurrencyDtoValidator _validator = new();

    [Fact]
    public void Should_Fail_When_ExchangeRate_Invalid()
    {
        var dto = new UpdateCurrencyDto
        {
            Name = "RON",
            ExchangeRateToBase = 0,
            IsActive = 1
        };

        _validator.Validate(dto).IsValid.Should().BeFalse();
    }
}
