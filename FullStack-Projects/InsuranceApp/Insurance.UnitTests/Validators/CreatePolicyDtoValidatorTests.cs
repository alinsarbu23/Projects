using FluentAssertions;
using Insurance.Application.DTOs.Policies;
using Insurance.Application.Validators.Policies;
using Xunit;

namespace Insurance.UnitTests.Validators;

public class CreatePolicyDtoValidatorTests
{
    private readonly CreatePolicyDtoValidator _validator = new();

    [Fact]
    public void Should_Fail_When_BasePremium_Is_Negative()
    {
        var dto = new CreatePolicyDto
        {
            BuildingId = Guid.NewGuid(),
            CurrencyId = Guid.NewGuid(),
            BasePremium = -100,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(10)
        };

        _validator.Validate(dto).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Fail_When_EndDate_In_Past()
    {
        var dto = new CreatePolicyDto
        {
            BuildingId = Guid.NewGuid(),
            CurrencyId = Guid.NewGuid(),
            BasePremium = 100,
            StartDate = DateTime.UtcNow.AddDays(-10),
            EndDate = DateTime.UtcNow.AddDays(-1)
        };

        _validator.Validate(dto).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Data_Is_Valid()
    {
        var dto = new CreatePolicyDto
        {
            BuildingId = Guid.NewGuid(),
            CurrencyId = Guid.NewGuid(),
            BasePremium = 100,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(10)
        };

        _validator.Validate(dto).IsValid.Should().BeTrue();
    }
}
