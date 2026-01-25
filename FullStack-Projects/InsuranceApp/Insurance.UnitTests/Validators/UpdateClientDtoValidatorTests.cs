using Xunit;
using FluentAssertions;
using Insurance.Application.DTOs.Clients;

namespace Insurance.UnitTests.Validators;

public class UpdateClientDtoValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldFail_WhenEmailIsInvalid(string? email)
    {
        var validator = new UpdateClientDtoValidator();

        var dto = new UpdateClientDto
        {
            Email = email!,
            Phone = "0700",
            Address = "Addr"
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ShouldPass_WhenDataIsValid()
    {
        var validator = new UpdateClientDtoValidator();

        var dto = new UpdateClientDto
        {
            Email = "test@test.ro",
            Phone = "0700",
            Address = "Addr"
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }
}
