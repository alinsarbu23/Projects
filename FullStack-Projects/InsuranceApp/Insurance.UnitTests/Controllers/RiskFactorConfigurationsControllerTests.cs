using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Insurance.Api.Controllers;
using Insurance.Application.DTOs.RiskFactors;
using Insurance.Application.Interfaces.Services;

namespace Insurance.UnitTests.Controllers;

    public class RiskFactorConfigurationsControllerTests
{
    private readonly Mock<IRiskFactorConfigurationService> _service;
    private readonly RiskFactorConfigurationsController _controller;

    public RiskFactorConfigurationsControllerTests()
    {
        _service = new Mock<IRiskFactorConfigurationService>();
        _controller = new RiskFactorConfigurationsController(_service.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAt()
    {
        var id = Guid.NewGuid();

        _service
            .Setup(x => x.CreateAsync(It.IsAny<CreateRiskFactorConfigurationDto>()))
            .ReturnsAsync(id);

        var result = await _controller.Create(new CreateRiskFactorConfigurationDto
        {
            Level = "City",
            ReferenceId = Guid.NewGuid(),
            AdjustmentPercentage = 5
        });

        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        _service
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<GetRiskFactorConfigurationsDto>());

        var result = await _controller.GetAll();

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        var service = new Mock<IRiskFactorConfigurationService>();

        service.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new GetRiskFactorConfigurationByIdDto());

        var controller = new RiskFactorConfigurationsController(service.Object);

        var result = await controller.GetById(Guid.NewGuid());

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent()
    {
        var service = new Mock<IRiskFactorConfigurationService>();
        var controller = new RiskFactorConfigurationsController(service.Object);

        var result = await controller.Update(Guid.NewGuid(), new UpdateRiskFactorConfigurationDto
        {
            AdjustmentPercentage = 5,
            IsActive = true
        });

        result.Should().BeOfType<NoContentResult>();
        service.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateRiskFactorConfigurationDto>()), Times.Once);
    }
}
