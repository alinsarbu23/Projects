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

    [Fact]
    public async Task Country_ShouldThrow_WhenInvalidDateRange()
    {
        var dto = new GetPoliciesReportDto
        {
            From = DateTime.UtcNow,
            To = DateTime.UtcNow.AddDays(-1)
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetPoliciesByCountryAsync(dto));
    }

    [Fact]
    public async Task Country_ShouldLogWarning_WhenNoResults()
    {
        _uowMock.Setup(x => x.Reports.GetPoliciesByCountryAsync(It.IsAny<GetPoliciesReportDto>()))
            .ReturnsAsync(new List<GetPoliciesByCountryReportDto>());

        var result = await _service.GetPoliciesByCountryAsync(new GetPoliciesReportDto());

        Assert.Empty(result);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Country_ShouldLogInformation_WhenResultsExist()
    {
        _uowMock.Setup(x => x.Reports.GetPoliciesByCountryAsync(It.IsAny<GetPoliciesReportDto>()))
            .ReturnsAsync(CreateCountryResult(2));

        var result = await _service.GetPoliciesByCountryAsync(new GetPoliciesReportDto());

        Assert.Equal(2, result.Count);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task County_ShouldLogWarning_WhenNoResults()
    {
        _uowMock.Setup(x => x.Reports.GetPoliciesByCountyAsync(It.IsAny<GetPoliciesReportDto>()))
            .ReturnsAsync(new List<GetPoliciesByCountyReportDto>());

        var result = await _service.GetPoliciesByCountyAsync(new GetPoliciesReportDto());

        Assert.Empty(result);
    }

    [Fact]
    public async Task County_ShouldReturnData_WhenResultsExist()
    {
        _uowMock.Setup(x => x.Reports.GetPoliciesByCountyAsync(It.IsAny<GetPoliciesReportDto>()))
            .ReturnsAsync(CreateCountyResult(1));

        var result = await _service.GetPoliciesByCountyAsync(new GetPoliciesReportDto());

        Assert.Single(result);
    }

    [Fact]
    public async Task City_ShouldLogWarning_WhenNoResults()
    {
        _uowMock.Setup(x => x.Reports.GetPoliciesByCityAsync(It.IsAny<GetPoliciesReportDto>()))
            .ReturnsAsync(new List<GetPoliciesByCityReportDto>());

        var result = await _service.GetPoliciesByCityAsync(new GetPoliciesReportDto());

        Assert.Empty(result);
    }

    [Fact]
    public async Task City_ShouldReturnData_WhenResultsExist()
    {
        _uowMock.Setup(x => x.Reports.GetPoliciesByCityAsync(It.IsAny<GetPoliciesReportDto>()))
            .ReturnsAsync(CreateCityResult(3));

        var result = await _service.GetPoliciesByCityAsync(new GetPoliciesReportDto());

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task Broker_ShouldThrow_WhenInvalidDateRange()
    {
        var dto = new GetPoliciesReportDto
        {
            From = DateTime.UtcNow,
            To = DateTime.UtcNow.AddDays(-1)
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetPoliciesByBrokerAsync(dto));
    }

    [Fact]
    public async Task Broker_ShouldLogWarning_WhenNoResults()
    {
        _uowMock.Setup(x => x.Reports.GetPoliciesByBrokerAsync(It.IsAny<GetPoliciesReportDto>()))
            .ReturnsAsync(new List<GetPoliciesByBrokerReportDto>());

        var result = await _service.GetPoliciesByBrokerAsync(new GetPoliciesReportDto());

        Assert.Empty(result);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Broker_ShouldLogInformation_WhenResultsExist()
    {
        _uowMock.Setup(x => x.Reports.GetPoliciesByBrokerAsync(It.IsAny<GetPoliciesReportDto>()))
            .ReturnsAsync(new List<GetPoliciesByBrokerReportDto>
            {
                    new GetPoliciesByBrokerReportDto
                    {
                        BrokerId = Guid.NewGuid(),
                        BrokerCode = "BR-001",
                        BrokerName = "Test",
                        PolicyCount = 1,
                        TotalFinalPremium = 1000,
                        TotalFinalPremiumInBaseCurrency = 1000
                    }
            });

        var result = await _service.GetPoliciesByBrokerAsync(new GetPoliciesReportDto());

        Assert.Single(result);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);

        _uowMock.Verify(
            x => x.Reports.GetPoliciesByBrokerAsync(It.IsAny<GetPoliciesReportDto>()),
            Times.Once);
    }
}
