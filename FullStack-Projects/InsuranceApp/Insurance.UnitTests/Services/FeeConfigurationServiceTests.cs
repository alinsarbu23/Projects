using FluentAssertions;
using Insurance.Application.DTOs.Fees;
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

public class FeeConfigurationServiceTests
{
    [Fact]
    public async Task UpdateAsync_WhenFeeNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.FeeConfigurations.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((FeeConfiguration?)null);

        var service = new FeeConfigurationService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<FeeConfigurationService>>());

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.UpdateAsync(Guid.NewGuid(), new UpdateFeeConfigurationDto()));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.FeeConfigurations.GetAllAsync())
            .ReturnsAsync(new List<FeeConfiguration>
            {
                new FeeConfiguration
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin Fee",
                    Type = FeeType.AdminFee,
                    Percentage = 5,
                    IsActive = true
                }
            });

        var service = new FeeConfigurationService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<FeeConfigurationService>>());

        var result = await service.GetAllAsync();

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Admin Fee");
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_Throws()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.FeeConfigurations.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((FeeConfiguration?)null);

        var service = new FeeConfigurationService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<FeeConfigurationService>>());

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task UpdateAsync_WhenFound_UpdatesAndSaves()
    {
        var fee = new FeeConfiguration
        {
            Id = Guid.NewGuid(),
            Name = "Admin Fee",
            Type = FeeType.AdminFee,
            Percentage = 5,
            IsActive = true
        };

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.FeeConfigurations.GetByIdAsync(fee.Id))
            .ReturnsAsync(fee);
        unitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var service = new FeeConfigurationService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<FeeConfigurationService>>());

        await service.UpdateAsync(fee.Id, new UpdateFeeConfigurationDto
        {
            Percentage = 10,
            IsActive = 0
        });

        fee.Percentage.Should().Be(10);
    }

    [Fact]
    public async Task CreateAsync_PersistsFeeConfiguration()
    {
        var uow = new Mock<IUnitOfWork>();
        var repo = new Mock<IFeeConfigurationRepository>();

        uow.Setup(x => x.FeeConfigurations).Returns(repo.Object);
        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var service = new FeeConfigurationService(
            uow.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<FeeConfigurationService>>());

        var id = await service.CreateAsync(new CreateFeeConfigurationDto
        {
            Name = "Admin Fee",
            Type = (int)FeeType.AdminFee,
            AppliesToRisk = (int)BuildingRiskType.None,
            Percentage = 5,
            EffectiveFrom = DateTime.UtcNow
        });

        id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsFee()
    {
        var id = Guid.NewGuid();
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork.Setup(x => x.FeeConfigurations.GetByIdAsync(id))
            .ReturnsAsync(new FeeConfiguration
            {
                Id = id,
                Name = "Admin Fee",
                Percentage = 5,
                IsActive = true
            });

        var service = new FeeConfigurationService(
            unitOfWork.Object,
            MapperFactory.Create(),
            Mock.Of<ILogger<FeeConfigurationService>>());

        var result = await service.GetByIdAsync(id);

        result.Name.Should().Be("Admin Fee");
    }
}
