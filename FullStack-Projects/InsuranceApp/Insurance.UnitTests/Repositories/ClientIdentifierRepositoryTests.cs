using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Repositories;
using Insurance.Domain.Models;
using Insurance.UnitTests.Helpers;

namespace Insurance.UnitTests.Repositories;

public class ClientIdentifierRepositoryTests
{
    private static InsuranceDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<InsuranceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InsuranceDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistClientIdentifier()
    {
        using var context = CreateDbContext();
        var repo = new ClientIdentifierRepository(context, MapperFactory.Create());

        var identifier = new ClientIdentifier
        {
            Id = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
            OldIdentifier = "OLD",
            NewIdentifier = "NEW",
            IdentifierType = "CNP",
            ChangedAt = DateTime.UtcNow
        };

        await repo.AddAsync(identifier);
        await context.SaveChangesAsync();

        var stored = await context.ClientIdentifiers.FindAsync(identifier.Id);

        stored.Should().NotBeNull();
        stored!.OldIdentifier.Should().Be("OLD");
        stored.NewIdentifier.Should().Be("NEW");
        stored.IdentifierType.Should().Be("CNP");
    }

    [Fact]
    public async Task GetLastChangeDateAsync_ShouldReturnLatestDate()
    {
        using var context = CreateDbContext();
        var repo = new ClientIdentifierRepository(context, MapperFactory.Create());

        var clientId = Guid.NewGuid();
        var older = DateTime.UtcNow.AddDays(-10);
        var newer = DateTime.UtcNow;

        context.ClientIdentifiers.AddRange(
            new Infrastructure.DAOs.ClientIdentifier
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                OldIdentifier = "A",
                NewIdentifier = "B",
                IdentifierType = "CNP",
                ChangedAt = older
            },
            new Infrastructure.DAOs.ClientIdentifier
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                OldIdentifier = "B",
                NewIdentifier = "C",
                IdentifierType = "CNP",
                ChangedAt = newer
            }
        );

        await context.SaveChangesAsync();

        var result = await repo.GetLastChangeDateAsync(clientId);

        result.Should().Be(newer);
    }

    [Fact]
    public async Task GetLastChangeDateAsync_ShouldReturnMinValue_WhenNoRecords()
    {
        using var context = CreateDbContext();
        var repo = new ClientIdentifierRepository(context, MapperFactory.Create());

        var result = await repo.GetLastChangeDateAsync(Guid.NewGuid());

        result.Should().Be(DateTime.MinValue);
    }
}
