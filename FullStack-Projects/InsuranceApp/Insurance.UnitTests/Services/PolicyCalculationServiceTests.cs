using FluentAssertions;
using Insurance.Application.Exceptions;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Services;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;
using Insurance.Domain.Models.Geography;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.UnitTests.Services;

public class PolicyCalculationServiceTests
{
    private static PolicyCalculationService CreateService(Mock<IUnitOfWork> unitOfWork)
    {
        return new PolicyCalculationService(
            unitOfWork.Object,
            Mock.Of<ILogger<PolicyCalculationService>>());
    }

    private static Building CreateValidBuilding()
    {
        return new Building
        {
            Id = Guid.NewGuid(),
            CityId = Guid.NewGuid(),
            BuildingTypeId = Guid.NewGuid(),
            City = new City
            {
                Id = Guid.NewGuid(),
                CountyId = Guid.NewGuid(),
                County = new County
                {
                    Id = Guid.NewGuid(),
                    CountryId = Guid.NewGuid()
                }
            }
        };
    }

    [Fact]
    public async Task CalculateFinalPremium_IgnoresRiskFee_WhenBuildingDoesNotMatch()
    {
        var building = new Building
        {
            Id = Guid.NewGuid(),
            CityId = Guid.NewGuid(),
            BuildingTypeId = Guid.NewGuid(),
            FloodRisk = false,
            EarthquakeRisk = false,
            City = new City
            {
                Id = Guid.NewGuid(),
                CountyId = Guid.NewGuid(),
                County = new County
                {
                    Id = Guid.NewGuid(),
                    CountryId = Guid.NewGuid()
                }
            }
        };

        var uow = new Mock<IUnitOfWork>();

        uow.Setup(x => x.Brokers.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Broker
            {
                Id = Guid.NewGuid(),
                BrokerCode = "BR-01",
                Name = "Broker",
                Status = BrokerStatus.Active,
                CommissionPercentage = 0
            });

        uow.Setup(x => x.Currencies.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Currency
            {
                Id = Guid.NewGuid(),
                Code = "RON",
                Name = "RON",
                ExchangeRateToBase = 1,
                IsActive = true
            });

        uow.Setup(x => x.FeeConfigurations.GetActiveAsync(null, It.IsAny<DateTime>()))
            .ReturnsAsync(new List<FeeConfiguration>
            {
            new FeeConfiguration
            {
                Id = Guid.NewGuid(),
                Name = "Flood Fee",
                Type = FeeType.RiskAdjustment,
                AppliesToRisk = BuildingRiskType.Flood,
                Percentage = 10,
                IsActive = true,
                EffectiveFrom = DateTime.UtcNow.AddDays(-1)
            }
            });

        uow.Setup(x => x.RiskFactorConfigurations.GetActiveForBuildingAsync(
            It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new List<RiskFactorConfiguration>());

        var service = new PolicyCalculationService(
            uow.Object,
            Mock.Of<ILogger<PolicyCalculationService>>());

        var result = await service.CalculateFinalPremiumAsync(
            1000,
            Guid.NewGuid(),
            Guid.NewGuid(),
            building);

        result.Should().Be(1000);
    }

    [Fact]
    public async Task CalculateFinalPremium_NoFees_NoRisks_ReturnsBasePremium()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork.Setup(x => x.Brokers.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Broker
            {
                Id = Guid.NewGuid(),
                BrokerCode = "BR-01",
                Name = "Test Broker",
                Status = BrokerStatus.Active,
                CommissionPercentage = 0
            });

        unitOfWork.Setup(x => x.Currencies.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Currency
            {
                Id = Guid.NewGuid(),
                Code = "RON",
                Name = "Romanian Leu",
                ExchangeRateToBase = 1,
                IsActive = true
            });

        unitOfWork.Setup(x => x.FeeConfigurations.GetActiveAsync(null, It.IsAny<DateTime>()))
            .ReturnsAsync([]);

        unitOfWork.Setup(x => x.RiskFactorConfigurations.GetActiveForBuildingAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync([]);

        var service = CreateService(unitOfWork);

        var result = await service.CalculateFinalPremiumAsync(
            1000,
            Guid.NewGuid(),
            Guid.NewGuid(),
            CreateValidBuilding());

        result.Should().Be(1000);
    }

    [Fact]
    public async Task CalculateFinalPremium_WithMultipleAdjustments_ReturnsCorrectValue()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork.Setup(x => x.Brokers.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Broker
            {
                Id = Guid.NewGuid(),
                BrokerCode = "BR-01",
                Name = "Test Broker",
                Status = BrokerStatus.Active,
                CommissionPercentage = 10
            });

        unitOfWork.Setup(x => x.Currencies.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Currency
            {
                Id = Guid.NewGuid(),
                Code = "EUR",
                Name = "Euro",
                ExchangeRateToBase = 1,
                IsActive = true
            });

        unitOfWork.Setup(x => x.FeeConfigurations.GetActiveAsync(null, It.IsAny<DateTime>()))
            .ReturnsAsync([
                new FeeConfiguration
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin Fee",
                    Type = FeeType.AdminFee,
                    Percentage = 5,
                    IsActive = true
                }
            ]);

        unitOfWork.Setup(x => x.RiskFactorConfigurations.GetActiveForBuildingAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync([
                new RiskFactorConfiguration
                {
                    Id = Guid.NewGuid(),
                    Level = RiskLevel.City,
                    AdjustmentPercentage = 5,
                    IsActive = true
                }
            ]);

        var service = CreateService(unitOfWork);

        var result = await service.CalculateFinalPremiumAsync(
            1000,
            Guid.NewGuid(),
            Guid.NewGuid(),
            CreateValidBuilding());

        result.Should().Be(1200);
    }

    [Fact]
    public async Task CalculateFinalPremium_ShouldThrow_WhenBrokerNotFound()
    {
        var uow = new Mock<IUnitOfWork>();

        uow.Setup(x => x.Brokers.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Broker?)null);

        var service = CreateService(uow);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.CalculateFinalPremiumAsync(
                1000,
                Guid.NewGuid(),
                Guid.NewGuid(),
                CreateValidBuilding()));
    }

    [Fact]
    public async Task CalculateFinalPremium_ShouldThrow_WhenBrokerInactive()
    {
        var uow = new Mock<IUnitOfWork>();

        uow.Setup(x => x.Brokers.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Broker
            {
                Id = Guid.NewGuid(),
                BrokerCode = "BR-01",
                Name = "Broker",
                Status = BrokerStatus.Inactive,
                CommissionPercentage = 0
            });

        uow.Setup(x => x.Currencies.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Currency
            {
                Id = Guid.NewGuid(),
                Code = "RON",
                Name = "RON",
                ExchangeRateToBase = 1,
                IsActive = true
            });

        uow.Setup(x => x.FeeConfigurations.GetActiveAsync(null, It.IsAny<DateTime>()))
            .ReturnsAsync([]);

        uow.Setup(x => x.RiskFactorConfigurations.GetActiveForBuildingAsync(
            It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync([]);

        var service = CreateService(uow);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CalculateFinalPremiumAsync(
                1000,
                Guid.NewGuid(),
                Guid.NewGuid(),
                CreateValidBuilding()));
    }

    [Fact]
    public async Task CalculateFinalPremium_ShouldThrow_WhenCurrencyInactive()
    {
        var uow = new Mock<IUnitOfWork>();

        uow.Setup(x => x.Brokers.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Broker
            {
                Id = Guid.NewGuid(),
                BrokerCode = "BR-01",
                Name = "Broker",
                Status = BrokerStatus.Active,
                CommissionPercentage = 0
            });

        uow.Setup(x => x.Currencies.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Currency
            {
                Id = Guid.NewGuid(),
                Code = "RON",
                Name = "RON",
                ExchangeRateToBase = 1,
                IsActive = false
            });

        var service = CreateService(uow);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CalculateFinalPremiumAsync(
                1000,
                Guid.NewGuid(),
                Guid.NewGuid(),
                CreateValidBuilding()));
    }

    [Fact]
    public async Task CalculateFinalPremium_WithOnlyAdminFee_ReturnsCorrectValue()
    {
        var uow = new Mock<IUnitOfWork>();

        uow.Setup(x => x.Brokers.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Broker
            {
                Id = Guid.NewGuid(),
                BrokerCode = "BR-01",
                Name = "Broker",
                Status = BrokerStatus.Active,
                CommissionPercentage = 0
            });

        uow.Setup(x => x.Currencies.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Currency
            {
                Id = Guid.NewGuid(),
                Code = "RON",
                Name = "RON",
                ExchangeRateToBase = 1,
                IsActive = true
            });

        uow.Setup(x => x.FeeConfigurations.GetActiveAsync(null, It.IsAny<DateTime>()))
            .ReturnsAsync([
                new FeeConfiguration
            {
                Id = Guid.NewGuid(),
                Name = "Admin Fee",
                Type = FeeType.AdminFee,
                Percentage = 10,
                IsActive = true
            }
            ]);

        uow.Setup(x => x.RiskFactorConfigurations.GetActiveForBuildingAsync(
            It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync([]);

        var service = CreateService(uow);

        var result = await service.CalculateFinalPremiumAsync(
            1000,
            Guid.NewGuid(),
            Guid.NewGuid(),
            CreateValidBuilding());

        result.Should().Be(1100);
    }

    [Fact]
    public async Task CalculateFinalPremium_WithFloodRisk_AppliesRiskFee()
    {
        var building = CreateValidBuilding();
        building.FloodRisk = true;

        var uow = new Mock<IUnitOfWork>();

        uow.Setup(x => x.Brokers.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Broker
            {
                Id = Guid.NewGuid(),
                BrokerCode = "BR-01",
                Name = "Broker",
                Status = BrokerStatus.Active,
                CommissionPercentage = 0
            });

        uow.Setup(x => x.Currencies.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Currency
            {
                Id = Guid.NewGuid(),
                Code = "RON",
                Name = "RON",
                ExchangeRateToBase = 1,
                IsActive = true
            });

        uow.Setup(x => x.FeeConfigurations.GetActiveAsync(null, It.IsAny<DateTime>()))
            .ReturnsAsync([
                new FeeConfiguration
            {
                Id = Guid.NewGuid(),
                Name = "Flood Fee",
                Type = FeeType.RiskAdjustment,
                AppliesToRisk = BuildingRiskType.Flood,
                Percentage = 10,
                IsActive = true,
                EffectiveFrom = DateTime.UtcNow.AddDays(-1)
            }
            ]);

        uow.Setup(x => x.RiskFactorConfigurations.GetActiveForBuildingAsync(
            It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync([]);

        var service = CreateService(uow);

        var result = await service.CalculateFinalPremiumAsync(
            1000,
            Guid.NewGuid(),
            Guid.NewGuid(),
            building);

        result.Should().Be(1100);
    }
}
