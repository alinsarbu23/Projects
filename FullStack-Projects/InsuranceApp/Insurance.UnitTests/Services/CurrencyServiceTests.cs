using FluentAssertions;
using Insurance.Application.DTOs.Currencies;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Services;
using Insurance.Domain.Models;
using Insurance.UnitTests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.UnitTests.Services;

public class CurrencyServiceTests
{

    [Fact]
    public async Task UpdateAsync_WhenCurrencyNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Currencies.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Currency?)null);

        var service = new CurrencyService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<CurrencyService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.UpdateAsync(Guid.NewGuid(), new UpdateCurrencyDto()));
    }

    [Fact]
    public async Task CreateAsync_CreatesActiveCurrency()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var repo = new Mock<ICurrencyRepository>();

        unitOfWork.Setup(x => x.Currencies).Returns(repo.Object);
        unitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var service = new CurrencyService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<CurrencyService>>());

        var id = await service.CreateAsync(new CreateCurrencyDto
        {
            Code = "RON",
            Name = "Romanian Leu",
            ExchangeRateToBase = 1
        });

        id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Currencies.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Currency?)null);

        var service = new CurrencyService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<CurrencyService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task UpdateAsync_DeactivateCurrencyUsedByActivePolicies_Throws()
    {
        var currencyId = Guid.NewGuid();

        var uow = new Mock<IUnitOfWork>();
        var currencyRepo = new Mock<ICurrencyRepository>();
        var policyRepo = new Mock<IPolicyRepository>();

        currencyRepo.Setup(x => x.GetByIdAsync(currencyId))
            .ReturnsAsync(new Currency
            {
                Id = currencyId,
                Code = "EUR",
                Name = "Euro",
                ExchangeRateToBase = 4.9m,
                IsActive = true
            });

        policyRepo.Setup(x => x.ExistsActivePolicyWithCurrencyAsync(currencyId))
            .ReturnsAsync(true);

        uow.Setup(x => x.Currencies).Returns(currencyRepo.Object);
        uow.Setup(x => x.Policies).Returns(policyRepo.Object);

        var service = new CurrencyService(
            uow.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<CurrencyService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.UpdateAsync(currencyId, new UpdateCurrencyDto
            {
                Name = "Euro",
                ExchangeRateToBase = 4.9m,
                IsActive = 0
            }));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsCurrencies()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork.Setup(x => x.Currencies.GetAllAsync())
            .ReturnsAsync(new List<Currency>
            {
            new Currency
            {
                Id = Guid.NewGuid(),
                Code = "RON",
                Name = "Romanian Leu",
                IsActive = true
            }
            });

        var service = new CurrencyService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<CurrencyService>>());

        var result = await service.GetAllAsync();

        result.Should().HaveCount(1);
        result[0].Code.Should().Be("RON");
    }
}
