using FluentAssertions;
using Insurance.Application.DTOs.RiskFactors;
using Insurance.Application.Exceptions;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Services;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;
using Insurance.UnitTests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.UnitTests.Services;

public class RiskFactorConfigurationServiceTests
{

    private static RiskFactorConfigurationService CreateService(
        Mock<IUnitOfWork> uow,
        Mock<IRiskFactorConfigurationRepository> repo)
    {
        uow.Setup(x => x.RiskFactorConfigurations)
           .Returns(repo.Object);

        return new RiskFactorConfigurationService(
            uow.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<RiskFactorConfigurationService>>());
    }

    [Fact]
    public async Task UpdateAsync_WhenRiskNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.RiskFactorConfigurations.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((RiskFactorConfiguration?)null);

        var service = new RiskFactorConfigurationService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<RiskFactorConfigurationService>>());

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.UpdateAsync(Guid.NewGuid(), new UpdateRiskFactorConfigurationDto()));
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateRiskFactorAndReturnId()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var repo = new Mock<IRiskFactorConfigurationRepository>();

        unitOfWork.Setup(x => x.RiskFactorConfigurations).Returns(repo.Object);
        unitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var service = new RiskFactorConfigurationService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<RiskFactorConfigurationService>>());

        var id = await service.CreateAsync(new CreateRiskFactorConfigurationDto
        {
            Level = (int)RiskLevel.City,
            ReferenceId = Guid.NewGuid(),
            AdjustmentPercentage = 5
        });

        id.Should().NotBe(Guid.Empty);
        repo.Verify(x => x.AddAsync(It.IsAny<RiskFactorConfiguration>()), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetRiskFactorByIdAsync_WhenNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.RiskFactorConfigurations.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((RiskFactorConfiguration?)null);

        var service = new RiskFactorConfigurationService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<RiskFactorConfigurationService>>());

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateAsync_WhenInvalidLevel_Throws()
    {
        var uow = new Mock<IUnitOfWork>();

        var service = new RiskFactorConfigurationService(
            uow.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<RiskFactorConfigurationService>>());

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(new CreateRiskFactorConfigurationDto
            {
                Level = 999,
                ReferenceId = Guid.NewGuid()
            }));
    }

    [Fact]
    public async Task CreateAsync_WhenLevelInvalid_Throws()
    {
        var uow = new Mock<IUnitOfWork>();
        var repo = new Mock<IRiskFactorConfigurationRepository>();

        var service = CreateService(uow, repo);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(new CreateRiskFactorConfigurationDto
            {
                Level = 999,
                ReferenceId = Guid.NewGuid(),
                AdjustmentPercentage = 5
            }));
    }

    [Fact]
    public async Task CreateAsync_WhenSaveFails_Throws()
    {
        var uow = new Mock<IUnitOfWork>();
        var repo = new Mock<IRiskFactorConfigurationRepository>();

        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

        var service = CreateService(uow, repo);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateAsync(new CreateRiskFactorConfigurationDto
            {
                Level = (int)RiskLevel.City,
                ReferenceId = Guid.NewGuid(),
                AdjustmentPercentage = 5
            }));
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_Throws()
    {
        var uow = new Mock<IUnitOfWork>();
        var repo = new Mock<IRiskFactorConfigurationRepository>();

        repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((RiskFactorConfiguration?)null);

        var service = CreateService(uow, repo);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.UpdateAsync(Guid.NewGuid(),
                new UpdateRiskFactorConfigurationDto()));
    }

    [Fact]
    public async Task UpdateAsync_WhenAlreadyActive_Throws()
    {
        var risk = new RiskFactorConfiguration
        {
            Id = Guid.NewGuid(),
            Level = RiskLevel.City,
            IsActive = true
        };

        var uow = new Mock<IUnitOfWork>();
        var repo = new Mock<IRiskFactorConfigurationRepository>();

        repo.Setup(x => x.GetByIdAsync(risk.Id))
            .ReturnsAsync(risk);

        var service = CreateService(uow, repo);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.UpdateAsync(risk.Id,
                new UpdateRiskFactorConfigurationDto
                {
                    IsActive = 1
                }));
    }

    [Fact]
    public async Task UpdateAsync_WhenSaveFails_Throws()
    {
        var risk = new RiskFactorConfiguration
        {
            Id = Guid.NewGuid(),
            Level = RiskLevel.City,
            IsActive = false
        };

        var uow = new Mock<IUnitOfWork>();
        var repo = new Mock<IRiskFactorConfigurationRepository>();

        repo.Setup(x => x.GetByIdAsync(risk.Id))
            .ReturnsAsync(risk);

        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

        var service = CreateService(uow, repo);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.UpdateAsync(risk.Id,
                new UpdateRiskFactorConfigurationDto
                {
                    IsActive = 1
                }));
    }

    [Fact]
    public async Task GetRiskFactorByIdAsync_WhenFound_ReturnsDto()
    {
        var risk = new RiskFactorConfiguration
        {
            Id = Guid.NewGuid(),
            Level = RiskLevel.City,
            AdjustmentPercentage = 5,
            IsActive = true
        };

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.RiskFactorConfigurations.GetByIdAsync(risk.Id))
            .ReturnsAsync(risk);

        var service = new RiskFactorConfigurationService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<RiskFactorConfigurationService>>());

        var dto = await service.GetByIdAsync(risk.Id);

        dto.Id.Should().Be(risk.Id);
    }



    [Fact]
    public async Task UpdateAsync_WhenRiskExists_UpdatesAndSaves()
    {
        var risk = new RiskFactorConfiguration
        {
            Id = Guid.NewGuid(),
            AdjustmentPercentage = 5,
            IsActive = true
        };

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.RiskFactorConfigurations.GetByIdAsync(risk.Id))
            .ReturnsAsync(risk);
        unitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var service = new RiskFactorConfigurationService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<RiskFactorConfigurationService>>());

        await service.UpdateAsync(risk.Id, new UpdateRiskFactorConfigurationDto
        {
            AdjustmentPercentage = 10
        });

        risk.AdjustmentPercentage.Should().Be(10);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRiskNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.RiskFactorConfigurations.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((RiskFactorConfiguration?)null);

        var service = new RiskFactorConfigurationService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<RiskFactorConfigurationService>>());

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.RiskFactorConfigurations.GetAllAsync())
            .ReturnsAsync(new List<RiskFactorConfiguration>
            {
                new RiskFactorConfiguration { Id = Guid.NewGuid(), IsActive = true }
            });

        var service = new RiskFactorConfigurationService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<RiskFactorConfigurationService>>());

        var result = await service.GetAllAsync();

        result.Should().HaveCount(1);
    }
}
