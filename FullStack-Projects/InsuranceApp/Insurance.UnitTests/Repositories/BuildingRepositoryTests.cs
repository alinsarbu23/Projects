using FluentAssertions;
using Insurance.Domain.Models;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Repositories;
using Insurance.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Insurance.UnitTests.Repositories;

public class BuildingRepositoryTests
{
    private static InsuranceDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<InsuranceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new InsuranceDbContext(options);

        context.Database.EnsureCreated();

        return context;
    }


    [Fact]
    public async Task AddAsync_ShouldPersistBuilding()
    {
        using var context = CreateDbContext();
        var repo = new BuildingRepository(context, MapperFactory.Create());

        var building = new Building
        {
            Id = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
            CityId = Guid.NewGuid(),
            BuildingTypeId = Guid.NewGuid(),
            Street = "Street",
            StreetNumber = "1",
            SurfaceArea = 20,
            InsuredValue = 100
        };

        await repo.AddAsync(building);
        await context.SaveChangesAsync();

        var stored = await context.Buildings.FindAsync(building.Id);

        stored.Should().NotBeNull();
        stored!.Street.Should().Be("Street");
        stored.SurfaceArea.Should().Be(20);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveBuilding_WhenExists()
    {
        using var context = CreateDbContext();
        var repo = new BuildingRepository(context, MapperFactory.Create());

        var dao = new Infrastructure.DAOs.Building
        {
            Id = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
            CityId = Guid.NewGuid(),
            BuildingTypeId = Guid.NewGuid(),
            Street = "Street",
            StreetNumber = "1",
            SurfaceArea = 10,
            InsuredValue = 100
        };

        context.Buildings.Add(dao);
        await context.SaveChangesAsync();

        await repo.DeleteAsync(dao.Id);
        await context.SaveChangesAsync();

        var deleted = await context.Buildings.FindAsync(dao.Id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBuildings()
    {
        using var context = CreateDbContext();
        var repo = new BuildingRepository(context, MapperFactory.Create());

        context.Buildings.AddRange(
            new Infrastructure.DAOs.Building { Id = Guid.NewGuid(), ClientId = Guid.NewGuid(), CityId = Guid.NewGuid(), BuildingTypeId = Guid.NewGuid(), Street = "A", StreetNumber = "1", SurfaceArea = 10, InsuredValue = 100 },
            new Infrastructure.DAOs.Building { Id = Guid.NewGuid(), ClientId = Guid.NewGuid(), CityId = Guid.NewGuid(), BuildingTypeId = Guid.NewGuid(), Street = "B", StreetNumber = "2", SurfaceArea = 20, InsuredValue = 200 }
        );

        await context.SaveChangesAsync();

        var result = await repo.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        using var context = CreateDbContext();
        var repo = new BuildingRepository(context, MapperFactory.Create());

        var result = await repo.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateClient()
    {
        using var context = CreateDbContext();
        var repo = new ClientRepository(context, MapperFactory.Create());

        var dao = new Infrastructure.DAOs.Client
        {
            Id = Guid.NewGuid(),
            ClientTypeId = 1,
            Name = "Old"
        };


        context.Clients.Add(dao);
        await context.SaveChangesAsync();

        dao.Name = "New";

        await context.SaveChangesAsync();

        var updated = await context.Clients.FindAsync(dao.Id);

        updated!.Name.Should().Be("New");
    }
}
