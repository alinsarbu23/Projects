using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Insurance.Api.Controllers;
using Insurance.Application.DTOs.Fees;
using Insurance.Application.Interfaces.Services;

namespace Insurance.UnitTests.Controllers;

public class FeeConfigurationsControllerTests
{
    private readonly Mock<IFeeConfigurationService> _service;
    private readonly FeeConfigurationsController _controller;

    public FeeConfigurationsControllerTests()
    {
        _service = new Mock<IFeeConfigurationService>();
        _controller = new FeeConfigurationsController(_service.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAt()
    {
        var feeId = Guid.NewGuid();

        _service
            .Setup(x => x.CreateAsync(It.IsAny<CreateFeeConfigurationDto>()))
            .ReturnsAsync(feeId);

        var result = await _controller.Create(new CreateFeeConfigurationDto
        {
            Name = "Admin Fee",
            Type = "AdminFee",
            Percentage = 5,
            EffectiveFrom = DateTime.UtcNow
        });

        var created = result as CreatedAtActionResult;
        created.Should().NotBeNull();
        created!.RouteValues!["feeId"].Should().Be(feeId);

        _service.Verify(x => x.CreateAsync(It.IsAny<CreateFeeConfigurationDto>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        _service
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<GetFeeConfigurationsDto>());

        var result = await _controller.GetAll();

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        var service = new Mock<IFeeConfigurationService>();

        service.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new GetFeeConfigurationByIdDto());

        var controller = new FeeConfigurationsController(service.Object);

        var result = await controller.GetById(Guid.NewGuid());

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent()
    {
        var service = new Mock<IFeeConfigurationService>();
        var controller = new FeeConfigurationsController(service.Object);

        var result = await controller.Update(Guid.NewGuid(), new UpdateFeeConfigurationDto
        {
            Percentage = 5,
            IsActive = true
        });

        result.Should().BeOfType<NoContentResult>();
        service.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateFeeConfigurationDto>()), Times.Once);
    }
}
