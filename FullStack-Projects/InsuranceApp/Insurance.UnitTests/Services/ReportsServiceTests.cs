using Insurance.Application.DTOs.Reports;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.UnitTests.Services;

public class ReportsServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<ILogger<ReportsService>> _loggerMock;
    private readonly ReportsService _service;

    public ReportsServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<ReportsService>>();
        _service = new ReportsService(_uowMock.Object, _loggerMock.Object);
    }

    private static List<GetPoliciesByCountryReportDto> CreateCountryResult(int count)
    {
        var list = new List<GetPoliciesByCountryReportDto>();
        for (int i = 0; i < count; i++)
        {
            list.Add(new GetPoliciesByCountryReportDto
            {
                CountryName = "Romania",
                CurrencyCode = "RON",
                PolicyCount = 1,
                TotalFinalPremium = 1000,
                TotalFinalPremiumInBaseCurrency = 1000
            });
        }
        return list;
    }

    private static List<GetPoliciesByCountyReportDto> CreateCountyResult(int count)
    {
        var list = new List<GetPoliciesByCountyReportDto>();
        for (int i = 0; i < count; i++)
        {
            list.Add(new GetPoliciesByCountyReportDto
            {
                CountyName = "Cluj",
                CurrencyCode = "RON",
                PolicyCount = 1,
                TotalFinalPremium = 1000,
                TotalFinalPremiumInBaseCurrency = 1000
            });
        }
        return list;
    }

    private static List<GetPoliciesByCityReportDto> CreateCityResult(int count)
    {
        var list = new List<GetPoliciesByCityReportDto>();
        for (int i = 0; i < count; i++)
        {
            list.Add(new GetPoliciesByCityReportDto
            {
                CityName = "Cluj-Napoca",
                CurrencyCode = "RON",
                PolicyCount = 1,
                TotalFinalPremium = 1000,
                TotalFinalPremiumInBaseCurrency = 1000
            });
        }
        return list;
    }

    private static List<GetPoliciesByBrokerReportDto> CreateBrokerResult(int count)
    {
        var list = new List<GetPoliciesByBrokerReportDto>();
        for (int i = 0; i < count; i++)
        {
            list.Add(new GetPoliciesByBrokerReportDto
            {
                BrokerId = Guid.NewGuid(),
                BrokerCode = "BR-001",
                BrokerName = "Test Broker",
                PolicyCount = 1,
                TotalFinalPremium = 1000,
                TotalFinalPremiumInBaseCurrency = 1000
            });
        }
        return list;
    }

    [Fact]
    public async Task GetPoliciesByCountryAsync_ShouldThrow_WhenInvalidDateRange()
    {
        var dto = new GetPoliciesReportDto
        {
            From = DateTime.UtcNow,
            To = DateTime.UtcNow.AddDays(-1)
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetPoliciesByCountryAsync(dto, 1, 10));
    }

    [Fact]
    public async Task GetPoliciesByCountryAsync_ShouldReturnEmpty_WhenNoResults()
    {
        _uowMock.Setup(x => x.Reports.GetPoliciesByCountryAsync(
                It.IsAny<GetPoliciesReportDto>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((new List<GetPoliciesByCountryReportDto>(), 0));

        var result = await _service.GetPoliciesByCountryAsync(new GetPoliciesReportDto(), 1, 10);

        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetPoliciesByCountryAsync_ShouldReturnPagedResult_WhenDataExists()
    {
        var data = CreateCountryResult(2);

        _uowMock.Setup(x => x.Reports.GetPoliciesByCountryAsync(
                It.IsAny<GetPoliciesReportDto>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((data, data.Count));

        var result = await _service.GetPoliciesByCountryAsync(new GetPoliciesReportDto(), 1, 10);

        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
    }

    [Fact]
    public async Task GetPoliciesByCountyAsync_ShouldReturnEmpty_WhenNoResults()
    {
        _uowMock.Setup(x => x.Reports.GetPoliciesByCountyAsync(
                It.IsAny<GetPoliciesReportDto>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((new List<GetPoliciesByCountyReportDto>(), 0));

        var result = await _service.GetPoliciesByCountyAsync(new GetPoliciesReportDto(), 1, 10);

        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetPoliciesByCountyAsync_ShouldReturnPagedResult_WhenDataExists()
    {
        var data = CreateCountyResult(1);

        _uowMock.Setup(x => x.Reports.GetPoliciesByCountyAsync(
                It.IsAny<GetPoliciesReportDto>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((data, data.Count));

        var result = await _service.GetPoliciesByCountyAsync(new GetPoliciesReportDto(), 1, 10);

        Assert.Equal(1, result.TotalCount);
        Assert.Single(result.Items);
    }

    [Fact]
    public async Task GetPoliciesByCityAsync_ShouldReturnEmpty_WhenNoResults()
    {
        _uowMock.Setup(x => x.Reports.GetPoliciesByCityAsync(
                It.IsAny<GetPoliciesReportDto>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((new List<GetPoliciesByCityReportDto>(), 0));

        var result = await _service.GetPoliciesByCityAsync(new GetPoliciesReportDto(), 1, 10);

        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetPoliciesByCityAsync_ShouldReturnPagedResult_WhenDataExists()
    {
        var data = CreateCityResult(3);

        _uowMock.Setup(x => x.Reports.GetPoliciesByCityAsync(
                It.IsAny<GetPoliciesReportDto>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((data, data.Count));

        var result = await _service.GetPoliciesByCityAsync(new GetPoliciesReportDto(), 1, 10);

        Assert.Equal(3, result.TotalCount);
        Assert.Equal(3, result.Items.Count);
    }

    [Fact]
    public async Task GetPoliciesByBrokerAsync_ShouldThrow_WhenInvalidDateRange()
    {
        var dto = new GetPoliciesReportDto
        {
            From = DateTime.UtcNow,
            To = DateTime.UtcNow.AddDays(-1)
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetPoliciesByBrokerAsync(dto, 1, 10));
    }

    [Fact]
    public async Task GetPoliciesByBrokerAsync_ShouldReturnEmpty_WhenNoResults()
    {
        _uowMock.Setup(x => x.Reports.GetPoliciesByBrokerAsync(
                It.IsAny<GetPoliciesReportDto>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((new List<GetPoliciesByBrokerReportDto>(), 0));

        var result = await _service.GetPoliciesByBrokerAsync(new GetPoliciesReportDto(), 1, 10);

        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetPoliciesByBrokerAsync_ShouldReturnPagedResult_WhenDataExists()
    {
        var data = CreateBrokerResult(1);

        _uowMock.Setup(x => x.Reports.GetPoliciesByBrokerAsync(
                It.IsAny<GetPoliciesReportDto>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((data, data.Count));

        var result = await _service.GetPoliciesByBrokerAsync(new GetPoliciesReportDto(), 1, 10);

        Assert.Equal(1, result.TotalCount);
        Assert.Single(result.Items);
    }
}
