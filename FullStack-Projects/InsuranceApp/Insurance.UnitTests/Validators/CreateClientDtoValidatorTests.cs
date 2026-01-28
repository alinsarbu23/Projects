using Xunit;
using FluentAssertions;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.Validators.Clients;
using Insurance.Domain.Enums;

namespace Insurance.UnitTests.Validators;

public class CreateClientDtoValidatorTests
{
    [Theory]
    [InlineData(ClientType.Individual, null, null)]
    [InlineData(ClientType.Company, null, null)]
    public void ShouldFail_WhenIdentifierMissing(
        ClientType type,
        string? cnp,
        string? cui)
    {
        var validator = new CreateClientDtoValidator();

        var dto = BaseDto(type, cnp, cui);

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(ClientType.Individual, null, "CUI")]
    [InlineData(ClientType.Company, "CNP", null)]
    public void ShouldFail_WhenWrongIdentifierProvided(
        ClientType type,
        string? cnp,
        string? cui)
    {
        var validator = new CreateClientDtoValidator();

        var dto = BaseDto(type, cnp, cui);

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ShouldFail_WhenBothIdentifiersProvided()
    {
        var validator = new CreateClientDtoValidator();

        var dto = BaseDto(
            ClientType.Individual,
            "1234567890123",
            "RO123"
        );

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ShouldFail_WhenCnpIsInvalid()
    {
        var validator = new CreateClientDtoValidator();

        var dto = BaseDto(
            ClientType.Individual,
            "123ABC",
            null
        );

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ShouldPass_WhenIndividualIsValid()
    {
        var validator = new CreateClientDtoValidator();

        var dto = BaseDto(
            ClientType.Individual,
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
            ClientType.Company,
            null,
            "RO123"
        );

        var result = validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    private static CreateClientDto BaseDto(
        ClientType type,
        string? cnp,
        string? cui)
    {
        return new CreateClientDto
        {
            ClientType = type,
            Name = "Client",
            Cnp = cnp,
            Cui = cui,
            Email = "test@test.ro",
            Phone = "0700",
            Address = "Addr"
        };
    }
}
