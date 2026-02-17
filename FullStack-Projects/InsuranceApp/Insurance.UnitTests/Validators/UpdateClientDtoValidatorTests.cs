using FluentAssertions;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.Validators.Clients;
using Xunit;

namespace Insurance.UnitTests.Validators;

public class UpdateClientDtoValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid")]
    public void ShouldFail_WhenEmailIsInvalid(string? email)
    {
        var validator = new UpdateClientDtoValidator();

        var dto = new UpdateClientDto
        {
            Name = "Client",
            Email = email!,
            Phone = "0700000000",
            Address = "Addr"
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ShouldFail_WhenNewIdentifierIsTooLong()
    {
        var validator = new UpdateClientIdentifierDtoValidator();

        var dto = new UpdateClientIdentifierDto
        {
            NewIdentifier = new string('1', 14)
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
            Name = "Client",
            Email = "client@test.ro",
            Phone = "0700000000",
            Address = "Addr"
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }
}
