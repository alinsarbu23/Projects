using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Insurance.Api.Controllers;
using Insurance.Application.DTOs.Policies;
using Insurance.Application.Interfaces.Services;

namespace Insurance.UnitTests.Controllers;
public class PoliciesControllerTests
{
    private readonly Mock<IPolicyService> _service;
    private readonly PoliciesController _controller;

    public PoliciesControllerTests()
    {
        _service = new Mock<IPolicyService>();
        _controller = new PoliciesController(_service.Object);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        _service
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new GetPolicyByIdDto());

        var result = await _controller.GetById(Guid.NewGuid());

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Activate_WhenCalled_ShouldReturnNoContent()
    {
        var service = new Mock<IPolicyService>();
        var controller = new PoliciesController(service.Object);

        var result = await controller.Activate(Guid.NewGuid());

        result.Should().BeOfType<NoContentResult>();
        service.Verify(x => x.ActivateAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Activate_WhenValidPolicyId_ShouldInvokeService()
    {
        var service = new Mock<IPolicyService>();
        var controller = new PoliciesController(service.Object);

        await controller.Activate(Guid.NewGuid());

        service.Verify(x => x.ActivateAsync(It.IsAny<Guid>()), Times.Once);
    }


    [Fact]
    public async Task Cancel_ShouldReturnNoContent()
    {
        var service = new Mock<IPolicyService>();
        var controller = new PoliciesController(service.Object);

        var result = await controller.Cancel(Guid.NewGuid(), new DeletePolicyDto
        {
            Reason = "Client request"
        });

        result.Should().BeOfType<NoContentResult>();
        service.Verify(x => x.CancelAsync(It.IsAny<Guid>(), It.IsAny<DeletePolicyDto>()), Times.Once);
    }
}
