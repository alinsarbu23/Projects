using FluentAssertions;
using Insurance.Application.DTOs.Policies;
using Insurance.Application.Exceptions;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Application.Services;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;
using Insurance.Domain.Models.Geography;
using Insurance.UnitTests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.UnitTests.Services;

public class PolicyServiceTests
{
    private static PolicyService CreateService(
        Mock<IUnitOfWork> uow,
        Mock<IPolicyCalculationService> calc)
    {
        return new PolicyService(
            uow.Object,
            calc.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());
    }

    [Fact]
    public async Task GetPolicyByIdAsync_WhenNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Policies.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Policy?)null);

        var service = new PolicyService(
            unitOfWork.Object,
            Mock.Of<IPolicyCalculationService>(),
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateDraftAsync_GeneratesPolicyNumber()
    {
        var broker = new Broker
        {
            Id = Guid.NewGuid(),
            BrokerCode = "BR",
            Name = "Broker",
            Status = BrokerStatus.Active
        };

        var building = new Building
        {
            Id = Guid.NewGuid(),
            City = new City
            {
                County = new County { CountryId = Guid.NewGuid() }
            }
        };

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Brokers.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(broker);
        unitOfWork.Setup(x => x.Buildings.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(building);
        unitOfWork.Setup(x => x.Policies.AddAsync(It.IsAny<Policy>()))
            .Callback<Policy>(p =>
            {
                p.PolicyNumber.Should().StartWith("POL-");
            });
        unitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var calc = new Mock<IPolicyCalculationService>();
        calc.Setup(x => x.CalculateFinalPremiumAsync(
                It.IsAny<decimal>(),
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<Building>()))
            .ReturnsAsync(100);

        var service = new PolicyService(
            unitOfWork.Object,
            calc.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());

        await service.CreateDraftAsync(new CreatePolicyDto
        {
            BuildingId = building.Id,
            CurrencyId = Guid.NewGuid(),
            BasePremium = 100,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(10)
        }, "broker@test.ro");
    }

    [Fact]
    public async Task SearchAsync_WhenBrokerNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Brokers.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Broker?)null);

        var service = new PolicyService(
            unitOfWork.Object,
            Mock.Of<IPolicyCalculationService>(),
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.SearchAsync(null, "x@test.ro", null, null, null, 1, 10));
    }

    [Fact]
    public async Task SearchAsync_ReturnsPagedPolicies()
    {
        var broker = new Broker
        {
            Id = Guid.NewGuid(),
            BrokerCode = "BR",
            Name = "Broker",
            Status = BrokerStatus.Active
        };

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Brokers.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(broker);
        unitOfWork.Setup(x => x.Policies.CountAsync(
                It.IsAny<Guid?>(),
                broker.Id,
                It.IsAny<PolicyStatus?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>()))
            .ReturnsAsync(2);
        unitOfWork.Setup(x => x.Policies.SearchAsync(
                It.IsAny<Guid?>(),
                broker.Id,
                It.IsAny<PolicyStatus?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                1,
                10))
            .ReturnsAsync(new List<Policy>
            {
            new Policy { PolicyNumber = "POL-1" },
            new Policy { PolicyNumber = "POL-2" }
            });

        var service = new PolicyService(
            unitOfWork.Object,
            Mock.Of<IPolicyCalculationService>(),
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());

        var result = await service.SearchAsync(null, "broker@test.ro", null, null, null, 1, 10);

        result.TotalCount.Should().Be(2);
        result.Items.Should().HaveCount(2);
    }


    [Fact]
    public async Task GetPolicyByIdAsync_WhenFound_ReturnsDto()
    {
        var policy = new Policy
        {
            Id = Guid.NewGuid(),
            PolicyNumber = "POL-123",
            Status = PolicyStatus.Active
        };

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Policies.GetByIdAsync(policy.Id))
            .ReturnsAsync(policy);

        var service = new PolicyService(
            unitOfWork.Object,
            Mock.Of<IPolicyCalculationService>(),
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());

        var dto = await service.GetByIdAsync(policy.Id);

        dto.PolicyNumber.Should().Be("POL-123");
    }


    [Fact]
    public async Task ActivateAsync_WhenPolicyAlreadyEnded_Throws()
    {
        var policy = new Policy
        {
            Id = Guid.NewGuid(),
            Status = PolicyStatus.Draft,
            EndDate = DateTime.UtcNow.Date.AddDays(-1)
        };

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Policies.GetByIdAsync(policy.Id))
            .ReturnsAsync(policy);

        var service = new PolicyService(
            unitOfWork.Object,
            Mock.Of<IPolicyCalculationService>(),
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.ActivateAsync(policy.Id));
    }

    [Fact]
    public async Task CancelAsync_WhenPolicyNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Policies.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Policy?)null);

        var service = new PolicyService(
            unitOfWork.Object,
            Mock.Of<IPolicyCalculationService>(),
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CancelAsync(Guid.NewGuid(), new DeletePolicyDto { Reason = "x" }));
    }

    [Fact]
    public async Task GetByIdAsync_WhenPolicyNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Policies.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Policy?)null);

        var service = new PolicyService(
            unitOfWork.Object,
            Mock.Of<IPolicyCalculationService>(),
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateDraftAsync_WhenBrokerNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Brokers.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Broker?)null);

        var service = new PolicyService(
            unitOfWork.Object,
            Mock.Of<IPolicyCalculationService>(),
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateDraftAsync(new CreatePolicyDto(), "x@test.ro"));
    }

    [Fact]
    public async Task ActivateAsync_WhenPolicyNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Policies.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Policy?)null);

        var service = new PolicyService(
            unitOfWork.Object,
            Mock.Of<IPolicyCalculationService>(),
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.ActivateAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task ActivateAsync_WhenPolicyNotDraft_Throws()
    {
        var policy = new Policy { Id = Guid.NewGuid(), Status = PolicyStatus.Active };

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Policies.GetByIdAsync(policy.Id))
            .ReturnsAsync(policy);

        var service = new PolicyService(
            unitOfWork.Object,
            Mock.Of<IPolicyCalculationService>(),
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.ActivateAsync(policy.Id));
    }

    [Fact]
    public async Task CancelAsync_WhenPolicyNotActive_Throws()
    {
        var policy = new Policy { Id = Guid.NewGuid(), Status = PolicyStatus.Draft };

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Policies.GetByIdAsync(policy.Id))
            .ReturnsAsync(policy);

        var service = new PolicyService(
            unitOfWork.Object,
            Mock.Of<IPolicyCalculationService>(),
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CancelAsync(policy.Id, new DeletePolicyDto { Reason = "x" }));
    }

    [Fact]
    public async Task ActivateAsync_DraftPolicy_Activates()
    {
        var uow = new Mock<IUnitOfWork>();
        var calc = new Mock<IPolicyCalculationService>();

        var policy = new Policy
        {
            Id = Guid.NewGuid(),
            Status = PolicyStatus.Draft,
            EndDate = DateTime.UtcNow.AddDays(1)
        };

        uow.Setup(x => x.Policies.GetByIdAsync(policy.Id))
            .ReturnsAsync(policy);

        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var service = CreateService(uow, calc);

        await service.ActivateAsync(policy.Id);

        policy.Status.Should().Be(PolicyStatus.Active);
        policy.ActivatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task CancelAsync_ActivePolicy_Cancels()
    {
        var uow = new Mock<IUnitOfWork>();
        var calc = new Mock<IPolicyCalculationService>();

        var policy = new Policy
        {
            Id = Guid.NewGuid(),
            Status = PolicyStatus.Active
        };

        uow.Setup(x => x.Policies.GetByIdAsync(policy.Id))
            .ReturnsAsync(policy);

        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var service = CreateService(uow, calc);

        await service.CancelAsync(policy.Id, new DeletePolicyDto
        {
            Reason = "Client request"
        });

        policy.Status.Should().Be(PolicyStatus.Cancelled);
        policy.CancelledAt.Should().NotBeNull();
        policy.CancellationReason.Should().Be("Client request");
    }

    [Fact]
    public async Task GetPolicyReportByBrokerAsync_ReturnsReport()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork.Setup(x => x.Policies.GetPolicyReportByBrokerAsync(
                It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
            .ReturnsAsync(new List<PolicyReportDto>
            {
            new PolicyReportDto { PolicyCount = 2 }
            });

        var service = new PolicyService(
            unitOfWork.Object,
            Mock.Of<IPolicyCalculationService>(),
            MapperFactory.Create(),
            Mock.Of<ILogger<PolicyService>>());

        var result = await service.GetPolicyReportByBrokerAsync(null, null);

        result.Should().HaveCount(1);
        result[0].PolicyCount.Should().Be(2);
    }
}
