using FluentAssertions;
using Insurance.Api.Controllers;
using Insurance.Application.DTOs.Policies;
using Insurance.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Insurance.UnitTests.Controllers;

public class PolicyReportsControllerTests
{
    [Fact]
    public async Task GetPolicyReportByBroker_ShouldReturnOk()
    {
        var service = new Mock<IPolicyService>();

        service.Setup(x => x.GetPolicyReportByBrokerAsync(
                It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
            .ReturnsAsync(new List<PolicyReportDto>());

        var controller = new PolicyReportsController(service.Object);

        var result = await controller.GetPolicyReportByBroker(null, null);

        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();
        ok!.StatusCode.Should().Be(200);
    }
}
