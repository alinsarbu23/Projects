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
            Name = "Test",
            Email = email!,
            Phone = "0700",
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
            Name = "Test",
            Email = "a@a.ro",
            Phone = "0700",
            Address = "Addr"
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }
}
