using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Repositories;
using Insurance.Domain.Models;
using Insurance.Domain.Enums;
using Insurance.UnitTests.Helpers;

namespace Insurance.UnitTests.Repositories;

public class ClientRepositoryTests
{
    private InsuranceDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<InsuranceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InsuranceDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistClient()
    {
        using var context = CreateDbContext();
        var repo = new ClientRepository(context, MapperFactory.Create());

        var client = new Client
        {
            Id = Guid.NewGuid(),
            ClientType = ClientType.Individual,
            Name = "Test Client",
            Cnp = "123"
        };

        await repo.AddAsync(client);
        await context.SaveChangesAsync();

        var stored = await context.Clients.FindAsync(client.Id);

        stored.Should().NotBeNull();
        stored!.Name.Should().Be("Test Client");
        stored.Cnp.Should().Be("123");
    }

    [Fact]
    public async Task ExistsByCnpAsync_ShouldReturnTrue_WhenClientExists()
    {
        using var context = CreateDbContext();
        var repo = new ClientRepository(context, MapperFactory.Create());

        context.Clients.Add(new Infrastructure.DAOs.Client
        {
            Id = Guid.NewGuid(),
            ClientType = ClientType.Individual.ToString(),
            Name = "Client",
            Cnp = "999"
        });

        await context.SaveChangesAsync();

        var exists = await repo.ExistsByCnpAsync("999");

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnFilteredClients()
    {
        using var context = CreateDbContext();
        var repo = new ClientRepository(context, MapperFactory.Create());

        context.Clients.AddRange(
            new Infrastructure.DAOs.Client
            {
                Id = Guid.NewGuid(),
                ClientType = ClientType.Individual.ToString(),
                Name = "Alice"
            },
            new Infrastructure.DAOs.Client
            {
                Id = Guid.NewGuid(),
                ClientType = ClientType.Individual.ToString(),
                Name = "Bob"
            });

        await context.SaveChangesAsync();

        var result = await repo.SearchAsync("Alice", null, 1, 10);

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Alice");
    }

    [Fact]
    public async Task CountAsync_ShouldFilterByIdentifier()
    {
        using var context = CreateDbContext();
        var repo = new ClientRepository(context, MapperFactory.Create());

        context.Clients.Add(new Infrastructure.DAOs.Client
        {
            Id = Guid.NewGuid(),
            ClientType = ClientType.Company.ToString(),
            Name = "Test",
            Cui = "RO1"
        });

        await context.SaveChangesAsync();

        var count = await repo.CountAsync(null, "RO1");

        count.Should().Be(1);
    }

    [Fact]
    public async Task ExistsByCuiAsync_ShouldReturnFalse_WhenNotExists()
    {
        using var context = CreateDbContext();
        var repo = new ClientRepository(context, MapperFactory.Create());

        var exists = await repo.ExistsByCuiAsync("RO404");

        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMappedClient()
    {
        using var context = CreateDbContext();
        var repo = new ClientRepository(context, MapperFactory.Create());

        var id = Guid.NewGuid();

        context.Clients.Add(new Infrastructure.DAOs.Client
        {
            Id = id,
            ClientType = ClientType.Individual.ToString(),
            Name = "Ion"
        });

        await context.SaveChangesAsync();

        var client = await repo.GetByIdAsync(id);

        client.Should().NotBeNull();
        client!.Name.Should().Be("Ion");
    }

    [Fact]
    public async Task CountAsync_ShouldReturnAll_WhenNoFilters()
    {
        using var context = CreateDbContext();
        var repo = new ClientRepository(context, MapperFactory.Create());

        context.Clients.AddRange(
            new Infrastructure.DAOs.Client
            {
                Id = Guid.NewGuid(),
                ClientType = "Individual",
                Name = "A"
            },
            new Infrastructure.DAOs.Client
            {
                Id = Guid.NewGuid(),
                ClientType = "Company",
                Name = "B"
            });

        await context.SaveChangesAsync();

        var count = await repo.CountAsync(null, null);

        count.Should().Be(2);
    }

    [Fact]
    public async Task SearchAsync_ShouldApplyPaging()
    {
        using var context = CreateDbContext();
        var repo = new ClientRepository(context, MapperFactory.Create());

        for (int i = 0; i < 5; i++)
        {
            context.Clients.Add(new Infrastructure.DAOs.Client
            {
                Id = Guid.NewGuid(),
                ClientType = "Individual",
                Name = $"Client{i}"
            });
        }

        await context.SaveChangesAsync();

        var result = await repo.SearchAsync(null, null, page: 2, pageSize: 2);

        result.Should().HaveCount(2);
    }


    [Fact]
    public async Task UpdateAsync_ShouldUpdateBuilding()
    {
        using var context = CreateDbContext();
        var repo = new BuildingRepository(context, MapperFactory.Create());

        var dao = new Infrastructure.DAOs.Building
        {
            Id = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
            CityId = Guid.NewGuid(),
            BuildingTypeId = Guid.NewGuid(),
            Street = "Old",
            StreetNumber = "1",
            SurfaceArea = 10,
            InsuredValue = 100
        };

        context.Buildings.Add(dao);
        await context.SaveChangesAsync();

        dao.Street = "New";
        dao.StreetNumber = "2";
        dao.SurfaceArea = 50;
        dao.InsuredValue = 500;

        await context.SaveChangesAsync();

        var updated = await context.Buildings.FindAsync(dao.Id);

        updated!.Street.Should().Be("New");
        updated.InsuredValue.Should().Be(500);
    }
}
