using FluentAssertions;
using Insurance.Domain.Models;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Repositories;
using Insurance.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Insurance.UnitTests.Repositories;

public class ClientRepositoryTests
{
    private static InsuranceDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<InsuranceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InsuranceDbContext(options);
    }

    private static void SeedClientTypes(InsuranceDbContext context)
    {
        context.ClientTypes.AddRange(
            new Infrastructure.DAOs.ClientType
            {
                Id = 1,
                Code = "INDIVIDUAL",
                Name = "Individual"
            },
            new Infrastructure.DAOs.ClientType
            {
                Id = 2,
                Code = "COMPANY",
                Name = "Company"
            }
        );

        context.SaveChanges();
    }

    [Fact]
    public async Task AddAsync_ShouldPersistClient()
    {
        using var context = CreateDbContext();
        var repo = new ClientRepository(context, MapperFactory.Create());

        var client = new Client
        {
            Id = Guid.NewGuid(),
            ClientTypeId = 1,
            Name = "Test Client",
            Cnp = "123",
            Email = "a@a.ro",
            Phone = "0700",
            Address = "Addr"
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
            ClientTypeId = 1,
            Name = "Client",
            Cnp = "999"
        });

        await context.SaveChangesAsync();

        var exists = await repo.ExistsByCnpAsync("999");

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingClient()
    {
        using var db = CreateDbContext();
        var repo = new ClientRepository(db, MapperFactory.Create());

        var dao = new Insurance.Infrastructure.DAOs.Client
        {
            Id = Guid.NewGuid(),
            ClientTypeId = 1,
            Name = "Old"
        };

        db.Clients.Add(dao);
        await db.SaveChangesAsync();

        var domain = MapperFactory.Create()
            .Map<Insurance.Domain.Models.Client>(dao);

        domain.Name = "New";

        await repo.UpdateAsync(domain);
        await db.SaveChangesAsync();

        db.Clients.First().Name.Should().Be("New");
    }

    [Fact]
    public async Task CountAsync_ShouldFilterByName()
    {
        using var db = CreateDbContext();

        db.Clients.AddRange(
            new Insurance.Infrastructure.DAOs.Client { Id = Guid.NewGuid(), ClientTypeId = 1, Name = "Ion" },
            new Insurance.Infrastructure.DAOs.Client { Id = Guid.NewGuid(), ClientTypeId = 1, Name = "Maria" }
        );

        await db.SaveChangesAsync();

        var repo = new ClientRepository(db, MapperFactory.Create());

        var count = await repo.CountAsync("Ion", null);

        count.Should().Be(1);
    }

    [Fact]
    public async Task SearchAsync_ShouldApplyPaging()
    {
        using var context = CreateDbContext();
        SeedClientTypes(context);

        var repo = new ClientRepository(context, MapperFactory.Create());

        for (int i = 0; i < 5; i++)
        {
            context.Clients.Add(new Infrastructure.DAOs.Client
            {
                Id = Guid.NewGuid(),
                ClientTypeId = 1,
                Name = $"Client{i}"
            });
        }

        await context.SaveChangesAsync();

        var result = await repo.SearchAsync(null, null, page: 2, pageSize: 2);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task CountAsync_ShouldFilterByIdentifier()
    {
        using var context = CreateDbContext();
        var repo = new ClientRepository(context, MapperFactory.Create());

        context.Clients.Add(new Infrastructure.DAOs.Client
        {
            Id = Guid.NewGuid(),
            ClientTypeId = 2,
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
        SeedClientTypes(context);

        var repo = new ClientRepository(context, MapperFactory.Create());

        var id = Guid.NewGuid();

        context.Clients.Add(new Infrastructure.DAOs.Client
        {
            Id = id,
            ClientTypeId = 1,
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
                ClientTypeId = 1,
                Name = "A"
            },
            new Infrastructure.DAOs.Client
            {
                Id = Guid.NewGuid(),
                ClientTypeId = 2,
                Name = "B"
            });

        await context.SaveChangesAsync();

        var count = await repo.CountAsync(null, null);

        count.Should().Be(2);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnFilteredClients()
    {
        using var context = CreateDbContext();
        SeedClientTypes(context);

        var repo = new ClientRepository(context, MapperFactory.Create());

        context.Clients.AddRange(
            new Infrastructure.DAOs.Client
            {
                Id = Guid.NewGuid(),
                ClientTypeId = 1,
                Name = "Alice"
            },
            new Infrastructure.DAOs.Client
            {
                Id = Guid.NewGuid(),
                ClientTypeId = 1,
                Name = "Bob"
            });

        await context.SaveChangesAsync();

        var result = await repo.SearchAsync("Alice", null, 1, 10);

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Alice");
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
