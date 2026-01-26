using FluentAssertions;
using Moq;
using Xunit;
using Insurance.Application.Services;
using Insurance.Application.DTOs.Buildings;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Models;
using Insurance.UnitTests.Helpers;

namespace Insurance.UnitTests.Services;

public class BuildingServiceTests
{
    private static BuildingService Create(Mock<IUnitOfWork> uow, Mock<IBuildingRepository> buildingsRepo)
    {
        uow.Setup(x => x.Buildings).Returns(buildingsRepo.Object);
        return new BuildingService(MapperFactory.Create(), uow.Object);
    }

    [Fact]
    public async Task GetBuildingByIdAsync_ShouldThrow_WhenNotFound()
    {
        var uow = new Mock<IUnitOfWork>();
        var repo = new Mock<IBuildingRepository>();

        var id = Guid.NewGuid();
        repo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Building?)null);

        var service = Create(uow, repo);

        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        {
            await service.GetBuildingByIdAsync(id);
        });
    }

    [Fact]
    public async Task GetBuildingByIdAsync_ShouldReturnDto_WhenFound()
    {
        var uow = new Mock<IUnitOfWork>();
        var repo = new Mock<IBuildingRepository>();

        var id = Guid.NewGuid();
        repo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(new Building
        {
            Id = id,
            Street = "S",
            StreetNumber = "1",
            SurfaceArea = 10,
            InsuredValue = 100,
            FloodRisk = true,
            EarthquakeRisk = false
        });

        var service = Create(uow, repo);

        var dto = await service.GetBuildingByIdAsync(id);

        dto.Id.Should().Be(id);
        dto.FloodRisk.Should().BeTrue();
        dto.EarthquakeRisk.Should().BeFalse();
    }

    [Fact]
    public async Task GetBuildingsForClientAsync_ShouldReturnList()
    {
        var uow = new Mock<IUnitOfWork>();
        var repo = new Mock<IBuildingRepository>();

        var clientId = Guid.NewGuid();
        repo.Setup(x => x.GetBuildingsByClientIdAsync(clientId))
            .ReturnsAsync(new List<Building>
            {
                new Building { Id = Guid.NewGuid(), InsuredValue = 111 },
                new Building { Id = Guid.NewGuid(), InsuredValue = 222 }
            });

        var service = Create(uow, repo);

        var result = await service.GetBuildingsForClientAsync(clientId);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateBuildingAsync_ShouldThrow_WhenNotFound()
    {
        var uow = new Mock<IUnitOfWork>();
        var repo = new Mock<IBuildingRepository>();

        var id = Guid.NewGuid();
        repo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Building?)null);

        var service = Create(uow, repo);

        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        {
            await service.UpdateBuildingAsync(id, new UpdateBuildingDto
            {
                Street = "S",
                StreetNumber = "1",
                ConstructionYear = 2000,
                NumberOfFloors = 1,
                SurfaceArea = 10,
                InsuredValue = 100,
                FloodRisk = true,
                EarthquakeRisk = true
            });
        });
    }

    [Fact]
    public async Task UpdateBuildingAsync_ShouldUpdate_AllFields()
    {
        var uow = new Mock<IUnitOfWork>();
        var repo = new Mock<IBuildingRepository>();

        var id = Guid.NewGuid();
        var entity = new Building
        {
            Id = id,
            Street = "Old",
            StreetNumber = "0",
            ConstructionYear = 1990,
            NumberOfFloors = 0,
            SurfaceArea = 1,
            InsuredValue = 1,
            FloodRisk = false,
            EarthquakeRisk = false
        };

        repo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(entity);
        repo.Setup(x => x.UpdateAsync(It.IsAny<Building>())).Returns(Task.CompletedTask);

        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var service = Create(uow, repo);

        var dto = new UpdateBuildingDto
        {
            Street = "NewStreet",
            StreetNumber = "10",
            ConstructionYear = 2020,
            NumberOfFloors = 3,
            SurfaceArea = 77,
            InsuredValue = 999,
            FloodRisk = true,
            EarthquakeRisk = true
        };

        await service.UpdateBuildingAsync(id, dto);

        entity.Street.Should().Be("NewStreet");
        entity.StreetNumber.Should().Be("10");
        entity.ConstructionYear.Should().Be(2020);
        entity.NumberOfFloors.Should().Be(3);
        entity.SurfaceArea.Should().Be(77);
        entity.InsuredValue.Should().Be(999);
        entity.FloodRisk.Should().BeTrue();
        entity.EarthquakeRisk.Should().BeTrue();

        repo.Verify(x => x.UpdateAsync(entity), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateBuildingAsync_ShouldThrow_WhenClientNotFound()
    {
        var uow = new Mock<IUnitOfWork>();
        var clients = new Mock<IClientRepository>();
        var buildings = new Mock<IBuildingRepository>();

        uow.Setup(x => x.Clients).Returns(clients.Object);
        uow.Setup(x => x.Buildings).Returns(buildings.Object);

        clients.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Client?)null);

        var service = new BuildingService(MapperFactory.Create(), uow.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        {
            await service.CreateBuildingAsync(Guid.NewGuid(), It.IsAny<CreateBuildingDto>());
        });
    }

    [Fact]
    public async Task CreateBuildingAsync_ShouldThrow_WhenSaveChangesIsZero()
    {
        var uow = new Mock<IUnitOfWork>();
        var clients = new Mock<IClientRepository>();
        var buildings = new Mock<IBuildingRepository>();

        uow.Setup(x => x.Clients).Returns(clients.Object);
        uow.Setup(x => x.Buildings).Returns(buildings.Object);
        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

        clients.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Client { Id = Guid.NewGuid() });

        var service = new BuildingService(MapperFactory.Create(), uow.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await service.CreateBuildingAsync(Guid.NewGuid(), new CreateBuildingDto
            {
                CityId = Guid.NewGuid(),
                BuildingTypeId = Guid.NewGuid(),
                Street = "S",
                StreetNumber = "1",
                SurfaceArea = 10,
                InsuredValue = 100
            });
        });
    }

    [Fact]
    public async Task UpdateBuildingAsync_ShouldThrow_WhenSaveChangesReturnsZero()
    {
        var uow = new Mock<IUnitOfWork>();
        var repo = new Mock<IBuildingRepository>();

        var id = Guid.NewGuid();
        var entity = new Building { Id = id };

        repo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(entity);
        repo.Setup(x => x.UpdateAsync(It.IsAny<Building>())).Returns(Task.CompletedTask);

        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

        var service = Create(uow, repo);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await service.UpdateBuildingAsync(id, new UpdateBuildingDto
            {
                Street = "S",
                StreetNumber = "1",
                ConstructionYear = 2020,
                NumberOfFloors = 2,
                SurfaceArea = 10,
                InsuredValue = 100,
                FloodRisk = false,
                EarthquakeRisk = false
            });
        });
    }
}
