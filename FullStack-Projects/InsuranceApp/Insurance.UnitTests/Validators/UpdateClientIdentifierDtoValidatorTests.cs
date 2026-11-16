using FluentAssertions;
using Insurance.Application.DTOs.Clients;
using Xunit;

namespace Insurance.UnitTests.Validators;

public class UpdateClientIdentifierDtoValidatorTests
{
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
