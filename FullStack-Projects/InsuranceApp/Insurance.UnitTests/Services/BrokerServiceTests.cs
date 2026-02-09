using FluentAssertions;
using Insurance.Application.DTOs.Brokers;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Services;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;
using Insurance.UnitTests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.UnitTests.Services;

public class BrokerServiceTests
{

    [Fact]
    public async Task CreateAsync_WhenDuplicateCode_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Brokers.GetByCodeAsync("BR"))
            .ReturnsAsync(new Broker
            {
                Id = Guid.NewGuid(),
                BrokerCode = "BR",
                Name = "Existing"
            });

        var service = new BrokerService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<BrokerService>>());

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(new CreateBrokerDto
            {
                BrokerCode = "BR",
                Name = "New",
                Email = "new@test.ro"
            }));
    }

    [Fact]
    public async Task CreateAsync_CreatesInactiveBroker()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var repo = new Mock<IBrokerRepository>();

        unitOfWork.Setup(x => x.Brokers).Returns(repo.Object);
        unitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var service = new BrokerService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<BrokerService>>());

        var id = await service.CreateAsync(new CreateBrokerDto
        {
            BrokerCode = "BR-NEW",
            Name = "Broker",
            Email = "broker@test.ro"
        });

        id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Brokers.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Broker?)null);

        var service = new BrokerService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<BrokerService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.UpdateAsync(Guid.NewGuid(), new UpdateBrokerDto()));
    }

    [Fact]
    public async Task DeactivateAsync_WhenFound_SetsInactive()
    {
        var broker = new Broker
        {
            Id = Guid.NewGuid(),
            BrokerCode = "BR",
            Name = "Broker",
            Status = BrokerStatus.Active
        };

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Brokers.GetByIdAsync(broker.Id))
            .ReturnsAsync(broker);
        unitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var service = new BrokerService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<BrokerService>>());

        await service.DeactivateAsync(broker.Id);

        broker.Status.Should().Be(BrokerStatus.Inactive);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Brokers.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Broker?)null);

        var service = new BrokerService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<BrokerService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task ActivateAsync_ActivatesBroker()
    {
        var broker = new Broker
        {
            Id = Guid.NewGuid(),
            BrokerCode = "BR-01",
            Name = "Test Broker",
            Status = BrokerStatus.Inactive
        };

        var repo = new Mock<IBrokerRepository>();
        repo.Setup(x => x.GetByIdAsync(broker.Id)).ReturnsAsync(broker);

        var uow = new Mock<IUnitOfWork>();
        uow.Setup(x => x.Brokers).Returns(repo.Object);
        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var service = new BrokerService(
            uow.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<BrokerService>>());

        await service.ActivateAsync(broker.Id);

        broker.Status.Should().Be(BrokerStatus.Active);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork.Setup(x => x.Brokers.GetAllAsync())
            .ReturnsAsync(new List<Broker>
            {
            new Broker
            {
                Id = Guid.NewGuid(),
                BrokerCode = "BR-01",
                Name = "Broker",
                Status = BrokerStatus.Active
            }
            });

        var service = new BrokerService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<BrokerService>>());

        var result = await service.GetAllAsync();

        result.Should().HaveCount(1);
        result[0].Status.Should().Be("Active");
    }
}
