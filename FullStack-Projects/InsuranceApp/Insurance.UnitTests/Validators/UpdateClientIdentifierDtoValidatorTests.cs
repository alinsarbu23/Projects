using Xunit;
using FluentAssertions;
using Insurance.Application.DTOs.Clients;

namespace Insurance.UnitTests.Validators;

public class UpdateClientIdentifierDtoValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldFail_WhenNewIdentifierIsMissing(string? value)
    {
        var validator = new UpdateClientIdentifierDtoValidator();

        var dto = new UpdateClientIdentifierDto
        {
            NewIdentifier = value!
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ShouldPass_WhenNewIdentifierIsProvided()
    {
        var validator = new UpdateClientIdentifierDtoValidator();

        var dto = new UpdateClientIdentifierDto
        {
            NewIdentifier = "NEW123"
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }
}
