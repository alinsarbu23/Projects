using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Insurance.Api.Controllers;
using Insurance.Application.DTOs.Brokers;
using Insurance.Application.Interfaces.Services;

namespace Insurance.UnitTests.Controllers;
public class BrokersControllerTests
{
    private readonly Mock<IBrokerService> _service;
    private readonly BrokersController _controller;

    public BrokersControllerTests()
    {
        _service = new Mock<IBrokerService>();
        _controller = new BrokersController(_service.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAt()
    {
        var id = Guid.NewGuid();

        _service
            .Setup(x => x.CreateAsync(It.IsAny<CreateBrokerDto>()))
            .ReturnsAsync(id);

        var result = await _controller.Create(new CreateBrokerDto
        {
            BrokerCode = "BR-01",
            Name = "Test Broker",
            Email = "broker@test.ro"
        });

        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task Activate_ShouldReturnNoContent()
    {
        var result = await _controller.Activate(Guid.NewGuid());

        result.Should().BeOfType<NoContentResult>();
        _service.Verify(x => x.ActivateAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Deactivate_ShouldReturnNoContent()
    {
        var service = new Mock<IBrokerService>();
        var controller = new BrokersController(service.Object);

        var result = await controller.Deactivate(Guid.NewGuid());

        result.Should().BeOfType<NoContentResult>();
        service.Verify(x => x.DeactivateAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        var service = new Mock<IBrokerService>();

        service.Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<GetBrokersDto>());

        var controller = new BrokersController(service.Object);

        var result = await controller.GetAll();

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        var service = new Mock<IBrokerService>();

        service.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new GetBrokerByIdDto());

        var controller = new BrokersController(service.Object);

        var result = await controller.GetById(Guid.NewGuid());

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent()
    {
        var service = new Mock<IBrokerService>();
        var controller = new BrokersController(service.Object);

        var result = await controller.Update(Guid.NewGuid(), new UpdateBrokerDto
        {
            Name = "Broker",
            CommissionPercentage = 1
        });

        result.Should().BeOfType<NoContentResult>();
        service.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateBrokerDto>()), Times.Once);
    }
}
