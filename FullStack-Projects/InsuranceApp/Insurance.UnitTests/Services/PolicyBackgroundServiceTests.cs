using Insurance.Api.BackgroundServices;
using Insurance.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.UnitTests.Services;

public class PolicyBackgroundServiceTests
{
    [Fact]
    public async Task ExecuteExpirationAsync_ShouldCallExpirePolicies()
    {
        var maintenanceMock = new Mock<IPolicyMaintenanceService>();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(x => x.GetService(typeof(IPolicyMaintenanceService)))
            .Returns(maintenanceMock.Object);

        var scopeMock = new Mock<IServiceScope>();
        scopeMock.Setup(x => x.ServiceProvider)
                 .Returns(serviceProviderMock.Object);

        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        scopeFactoryMock.Setup(x => x.CreateScope())
                        .Returns(scopeMock.Object);

        var loggerMock = new Mock<ILogger<PolicyBackgroundService>>();

        var service = new PolicyBackgroundService(
            scopeFactoryMock.Object,
            loggerMock.Object);

        await service.ExecuteExpirationAsync();

        maintenanceMock.Verify(x => x.ExpirePoliciesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteExpirationAsync_WhenExceptionThrown_ShouldPropagate()
    {
        var maintenanceMock = new Mock<IPolicyMaintenanceService>();
        maintenanceMock
            .Setup(x => x.ExpirePoliciesAsync())
            .ThrowsAsync(new Exception("boom"));

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(x => x.GetService(typeof(IPolicyMaintenanceService)))
            .Returns(maintenanceMock.Object);

        var scopeMock = new Mock<IServiceScope>();
        scopeMock.Setup(x => x.ServiceProvider)
                 .Returns(serviceProviderMock.Object);

        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        scopeFactoryMock.Setup(x => x.CreateScope())
                        .Returns(scopeMock.Object);

        var loggerMock = new Mock<ILogger<PolicyBackgroundService>>();

        var service = new PolicyBackgroundService(
            scopeFactoryMock.Object,
            loggerMock.Object);

        await Assert.ThrowsAsync<Exception>(() =>
            service.ExecuteExpirationAsync());
    }
}
