using Insurance.Api.Controllers;
using Insurance.Application.DTOs.Reports;
using Insurance.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Insurance.UnitTests.Controllers;

public class ReportsControllerTests
{
    private readonly Mock<IReportsService> _serviceMock;
    private readonly ReportsController _controller;

    public ReportsControllerTests()
    {
        _serviceMock = new Mock<IReportsService>();
        _controller = new ReportsController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetPoliciesByBrokerAsync_ShouldReturnOk()
    {
        _serviceMock
            .Setup(x => x.GetPoliciesByBrokerAsync(It.IsAny<GetPoliciesReportDto>()))
            .ReturnsAsync(new List<GetPoliciesByBrokerReportDto>());

        var result = await _controller.GetPoliciesByBrokerAsync(new GetPoliciesReportDto());

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IReadOnlyList<GetPoliciesByBrokerReportDto>>(okResult.Value);
        _serviceMock.Verify(x => x.GetPoliciesByBrokerAsync(It.IsAny<GetPoliciesReportDto>()), Times.Once);
    }

    [Fact]
    public async Task GetPoliciesByCountryAsync_ShouldReturnOk()
    {
        _serviceMock
            .Setup(x => x.GetPoliciesByCountryAsync(It.IsAny<GetPoliciesReportDto>()))
            .ReturnsAsync(new List<GetPoliciesByCountryReportDto>());

        var result = await _controller.GetPoliciesByCountryAsync(new GetPoliciesReportDto());

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IReadOnlyList<GetPoliciesByCountryReportDto>>(okResult.Value);
        _serviceMock.Verify(x => x.GetPoliciesByCountryAsync(It.IsAny<GetPoliciesReportDto>()), Times.Once);
    }

    [Fact]
    public async Task GetPoliciesByCountyAsync_ShouldReturnOk()
    {
        _serviceMock
            .Setup(x => x.GetPoliciesByCountyAsync(It.IsAny<GetPoliciesReportDto>()))
            .ReturnsAsync(new List<GetPoliciesByCountyReportDto>());

        var result = await _controller.GetPoliciesByCountyAsync(new GetPoliciesReportDto());

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IReadOnlyList<GetPoliciesByCountyReportDto>>(okResult.Value);
        _serviceMock.Verify(x => x.GetPoliciesByCountyAsync(It.IsAny<GetPoliciesReportDto>()), Times.Once);
    }

    [Fact]
    public async Task GetPoliciesByCityAsync_ShouldReturnOk()
    {
        _serviceMock
            .Setup(x => x.GetPoliciesByCityAsync(It.IsAny<GetPoliciesReportDto>()))
            .ReturnsAsync(new List<GetPoliciesByCityReportDto>());

        var result = await _controller.GetPoliciesByCityAsync(new GetPoliciesReportDto());

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IReadOnlyList<GetPoliciesByCityReportDto>>(okResult.Value);
        _serviceMock.Verify(x => x.GetPoliciesByCityAsync(It.IsAny<GetPoliciesReportDto>()), Times.Once);
    }
}
