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
    public async Task GetPoliciesByCountry_ShouldReturnOk_WithPagedResult()
    {
        var pagedResult = new PagedResultDto<GetPoliciesByCountryReportDto>
        {
            Page = 1,
            PageSize = 10,
            TotalCount = 2,
            Items = new List<GetPoliciesByCountryReportDto>
            {
                new()
                {
                    CountryName = "Romania",
                    CurrencyCode = "RON",
                    PolicyCount = 2,
                    TotalFinalPremium = 2000,
                    TotalFinalPremiumInBaseCurrency = 2000
                }
            }
        };

        _serviceMock
            .Setup(x => x.GetPoliciesByCountryAsync(It.IsAny<GetPoliciesReportDto>(), 1, 10))
            .ReturnsAsync(pagedResult);

        var result = await _controller.GetPoliciesByCountry(new GetPoliciesReportDto(), 1, 10);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<PagedResultDto<GetPoliciesByCountryReportDto>>(okResult.Value);

        Assert.Equal(1, value.Page);
        Assert.Equal(10, value.PageSize);
        Assert.Equal(2, value.TotalCount);
        Assert.Single(value.Items);
    }

    [Fact]
    public async Task GetPoliciesByCity_ShouldReturnOk_WithPagedResult()
    {
        var pagedResult = new PagedResultDto<GetPoliciesByCityReportDto>
        {
            Page = 1,
            PageSize = 5,
            TotalCount = 3,
            Items = new List<GetPoliciesByCityReportDto>
            {
                new()
                {
                    CityName = "Cluj-Napoca",
                    CurrencyCode = "RON",
                    PolicyCount = 3,
                    TotalFinalPremium = 3000,
                    TotalFinalPremiumInBaseCurrency = 3000
                }
            }
        };

        _serviceMock
            .Setup(x => x.GetPoliciesByCityAsync(It.IsAny<GetPoliciesReportDto>(), 1, 5))
            .ReturnsAsync(pagedResult);

        var result = await _controller.GetPoliciesByCity(new GetPoliciesReportDto(), 1, 5);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<PagedResultDto<GetPoliciesByCityReportDto>>(okResult.Value);

        Assert.Equal(3, value.TotalCount);
        Assert.Single(value.Items);
    }

    [Fact]
    public async Task GetPoliciesByBroker_ShouldCallService_WithCorrectParameters()
    {
        var pagedResult = new PagedResultDto<GetPoliciesByBrokerReportDto>
        {
            Page = 2,
            PageSize = 5,
            TotalCount = 10,
            Items = new List<GetPoliciesByBrokerReportDto>()
        };

        _serviceMock
            .Setup(x => x.GetPoliciesByBrokerAsync(It.IsAny<GetPoliciesReportDto>(), 2, 5))
            .ReturnsAsync(pagedResult);

        await _controller.GetPoliciesByBroker(new GetPoliciesReportDto(), 2, 5);

        _serviceMock.Verify(
            x => x.GetPoliciesByBrokerAsync(It.IsAny<GetPoliciesReportDto>(), 2, 5),
            Times.Once);
    }

    [Fact]
    public async Task GetPoliciesByCounty_ShouldReturnOk()
    {
        var pagedResult = new PagedResultDto<GetPoliciesByCountyReportDto>
        {
            Page = 1,
            PageSize = 10,
            TotalCount = 0,
            Items = new List<GetPoliciesByCountyReportDto>()
        };

        _serviceMock
            .Setup(x => x.GetPoliciesByCountyAsync(It.IsAny<GetPoliciesReportDto>(), 1, 10))
            .ReturnsAsync(pagedResult);

        var result = await _controller.GetPoliciesByCounty(new GetPoliciesReportDto(), 1, 10);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<PagedResultDto<GetPoliciesByCountyReportDto>>(okResult.Value);

        Assert.Empty(value.Items);
        Assert.Equal(0, value.TotalCount);
    }
}
