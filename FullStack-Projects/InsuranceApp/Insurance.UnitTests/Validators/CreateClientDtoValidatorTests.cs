using Xunit;
using FluentAssertions;
using Insurance.Application.Validators.Clients;
using Insurance.Application.DTOs.Clients;
using Insurance.Domain.Enums;

namespace Insurance.UnitTests.Validators;

public class CreateClientDtoValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldFail_WhenNameIsMissing(string? name)
    {
        var validator = new CreateClientDtoValidator();

        var dto = new CreateClientDto
        {
            ClientType = ClientType.Individual,
            Name = name!,
            Cnp = "123",
            Email = "test@test.ro",
            Phone = "0700",
            Address = "Addr"
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(ClientType.Individual, null, "CUI")]
    [InlineData(ClientType.Company, "CNP", null)]
    public void ShouldFail_WhenIdentifierMissingForClientType(
        ClientType type,
        string? cnp,
        string? cui)
    {
        var validator = new CreateClientDtoValidator();

        var dto = new CreateClientDto
        {
            ClientType = type,
            Name = "Client",
            Cnp = cnp,
            Cui = cui,
            Email = "test@test.ro",
            Phone = "0700",
            Address = "Addr"
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ShouldPass_WhenDataIsValid()
    {
        var validator = new CreateClientDtoValidator();

        var dto = new CreateClientDto
        {
            ClientType = ClientType.Company,
            Name = "Company",
            Cui = "RO123",
            Email = "test@test.ro",
            Phone = "0700",
            Address = "Addr"
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }
}
