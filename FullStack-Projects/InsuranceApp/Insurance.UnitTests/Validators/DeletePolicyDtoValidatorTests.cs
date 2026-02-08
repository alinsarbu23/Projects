using FluentAssertions;
using Insurance.Application.DTOs.Policies;
using Insurance.Application.Validators.Policies;
using Xunit;

namespace Insurance.UnitTests.Validators;

public class DeletePolicyDtoValidatorTests
{
    private readonly DeletePolicyDtoValidator _validator = new();

    [Fact]
    public void Should_Fail_When_Reason_Is_Empty()
    {
        var dto = new DeletePolicyDto { Reason = "" };

        _validator.Validate(dto).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Reason_Is_Provided()
    {
        var dto = new DeletePolicyDto { Reason = "Client request" };

        _validator.Validate(dto).IsValid.Should().BeTrue();
    }
}
