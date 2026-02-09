using FluentAssertions;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.Validators.Clients;
using Xunit;

namespace Insurance.UnitTests.Validators;

public class CreateClientDtoValidatorTests
{
    [Theory]
    [InlineData(1, null, null)]
    [InlineData(2, null, null)]
    public void ShouldFail_WhenIdentifierMissing(
        int clientTypeId,
        string? cnp,
        string? cui)
    {
        var validator = new CreateClientDtoValidator();

        var dto = BaseDto(clientTypeId, cnp, cui);

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(1, null, "CUI")]
    [InlineData(2, "CNP", null)]
    public void ShouldFail_WhenWrongIdentifierProvided(
        int clientTypeId,
        string? cnp,
        string? cui)
    {
        var validator = new CreateClientDtoValidator();

        var dto = BaseDto(clientTypeId, cnp, cui);

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ShouldFail_WhenBothIdentifiersProvided()
    {
        var validator = new CreateClientDtoValidator();

        var dto = BaseDto(
            1,
            "1234567890123",
            "RO123"
        );

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ShouldPass_WhenIndividualIsValid()
    {
        var validator = new CreateClientDtoValidator();

        var dto = BaseDto(
            1,
            "1234567890123",
            null
        );

        var result = validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ShouldPass_WhenCompanyIsValid()
    {
        var validator = new CreateClientDtoValidator();

        var dto = BaseDto(
            2,
            null,
            "RO123"
        );

        var result = validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    private static CreateClientDto BaseDto(
        int clientTypeId,
        string? cnp,
        string? cui)
    {
        return new CreateClientDto
        {
            ClientTypeId = clientTypeId,
            Name = "Client",
            Cnp = cnp,
            Cui = cui,
            Email = "test@test.ro",
            Phone = "0700",
            Address = "Addr"
        };
    }
}
