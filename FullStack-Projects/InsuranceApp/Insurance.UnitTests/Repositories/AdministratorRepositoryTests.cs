using FluentAssertions;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Repositories;
using Insurance.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Insurance.UnitTests.Repositories;

public class AdministratorRepositoryTests
{
    private static InsuranceDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<InsuranceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InsuranceDbContext(options);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAdministrator_WhenExists()
    {
        using var context = CreateDbContext();

        var adminId = Guid.NewGuid();

        context.Administrators.Add(new Insurance.Infrastructure.DAOs.Administrator
        {
            Id = adminId,
            Name = "System Admin",
            Email = "admin@test.ro",
            Role = "Admin"
        });

        await context.SaveChangesAsync();

        var repo = new AdministratorRepository(context, MapperFactory.Create());

        var result = await repo.GetByIdAsync(adminId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(adminId);
        result.Email.Should().Be("admin@test.ro");
        result.Role.Should().Be(Insurance.Domain.Enums.AdminRole.Admin);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        using var context = CreateDbContext();
        var repo = new AdministratorRepository(context, MapperFactory.Create());

        var result = await repo.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnAdministrator_WhenExists()
    {
        using var context = CreateDbContext();

        context.Administrators.Add(new Insurance.Infrastructure.DAOs.Administrator
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Email = "admin@test.ro",
            Role = "Admin"
        });

        await context.SaveChangesAsync();

        var repo = new AdministratorRepository(context, MapperFactory.Create());

        var result = await repo.GetByEmailAsync("admin@test.ro");

        result.Should().NotBeNull();
        result!.Email.Should().Be("admin@test.ro");
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenNotExists()
    {
        using var context = CreateDbContext();
        var repo = new AdministratorRepository(context, MapperFactory.Create());

        var result = await repo.GetByEmailAsync("missing@test.ro");

        result.Should().BeNull();
    }
}
