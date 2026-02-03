using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Insurance.Application.Services;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Models;
using Insurance.UnitTests.Helpers;

namespace Insurance.UnitTests.Services;

public class ClientServiceTests
{
    private static ClientService CreateService(
        Mock<IUnitOfWork> uow,
        Mock<IClientRepository> clientsRepo,
        Mock<IBuildingRepository>? buildingsRepo = null,
        Mock<IClientIdentifierRepository>? identifiersRepo = null,
        Mock<ILogger<ClientService>>? logger = null)
    {
        buildingsRepo ??= new Mock<IBuildingRepository>();
        identifiersRepo ??= new Mock<IClientIdentifierRepository>();
        logger ??= new Mock<ILogger<ClientService>>();

        uow.Setup(x => x.Clients).Returns(clientsRepo.Object);
        uow.Setup(x => x.Buildings).Returns(buildingsRepo.Object);
        uow.Setup(x => x.ClientIdentifiers).Returns(identifiersRepo.Object);

        return new ClientService(
            MapperFactory.Create(),
            uow.Object,
            logger.Object);
    }

    [Fact]
    public async Task CreateClientAsync_ShouldThrow_WhenCuiAlreadyExists()
    {
        var uow = new Mock<IUnitOfWork>();
        var clients = new Mock<IClientRepository>();

        clients.Setup(x => x.ExistsByCuiAsync("RO1")).ReturnsAsync(true);
        uow.Setup(x => x.Clients).Returns(clients.Object);

        var service = CreateService(uow, clients);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateClientAsync(new CreateClientDto
            {
                ClientTypeId = 2, // Company
                Name = "Company",
                Cui = "RO1",
                Email = "a@a.ro",
                Phone = "0700",
                Address = "Addr"
            }));
    }

    [Fact]
    public async Task CreateClientAsync_ShouldThrow_WhenSaveChangesReturnsZero()
    {
        var uow = new Mock<IUnitOfWork>();
        var clients = new Mock<IClientRepository>();

        clients.Setup(x => x.ExistsByCuiAsync(It.IsAny<string>())).ReturnsAsync(false);
        clients.Setup(x => x.ExistsByCnpAsync(It.IsAny<string>())).ReturnsAsync(false);
        clients.Setup(x => x.AddAsync(It.IsAny<Client>())).Returns(Task.CompletedTask);

        uow.Setup(x => x.Clients).Returns(clients.Object);
        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

        var service = CreateService(uow, clients);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateClientAsync(new CreateClientDto
            {
                ClientTypeId = 2,
                Name = "Company",
                Cui = "RO1",
                Email = "a@a.ro",
                Phone = "0700",
                Address = "Addr"
            }));
    }

    [Fact]
    public async Task GetClientByIdAsync_ShouldPopulateIdentifierAndBuildings()
    {
        var uow = new Mock<IUnitOfWork>();
        var clients = new Mock<IClientRepository>();
        var buildings = new Mock<IBuildingRepository>();

        var clientId = Guid.NewGuid();

        clients.Setup(x => x.GetByIdAsync(clientId)).ReturnsAsync(new Client
        {
            Id = clientId,
            ClientTypeId = 1, // Individual
            Name = "Ion",
            Cnp = "123",
            Email = "a@a.ro",
            Phone = "0700",
            Address = "Addr"
        });

        buildings.Setup(x => x.GetBuildingsByClientIdAsync(clientId))
            .ReturnsAsync(new List<Building>
            {
                new Building { Id = Guid.NewGuid(), InsuredValue = 100 },
                new Building { Id = Guid.NewGuid(), InsuredValue = 200 }
            });

        uow.Setup(x => x.Clients).Returns(clients.Object);
        uow.Setup(x => x.Buildings).Returns(buildings.Object);

        var service = CreateService(uow, clients, buildings);

        var dto = await service.GetClientByIdAsync(clientId);

        dto.Id.Should().Be(clientId);
        dto.Identifier.Should().Be("123");
        dto.Buildings.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateClientAsync_ShouldThrow_WhenSaveChangesReturnsZero()
    {
        var uow = new Mock<IUnitOfWork>();
        var clients = new Mock<IClientRepository>();

        var clientId = Guid.NewGuid();

        clients.Setup(x => x.GetByIdAsync(clientId))
            .ReturnsAsync(new Client { Id = clientId });

        clients.Setup(x => x.UpdateAsync(It.IsAny<Client>()))
            .Returns(Task.CompletedTask);

        uow.Setup(x => x.Clients).Returns(clients.Object);
        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

        var service = CreateService(uow, clients);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.UpdateClientAsync(clientId, new UpdateClientDto
            {
                Name = "Test",
                Email = "a@a.ro",
                Phone = "0700",
                Address = "Addr"
            }));
    }

    [Theory]
    [InlineData(1, "NEW_CNP")]
    [InlineData(2, "NEW_CUI")]
    public async Task UpdateClientIdentifierAsync_ShouldUpdateCorrectField(
        int clientTypeId,
        string newValue)
    {
        var uow = new Mock<IUnitOfWork>();
        var clients = new Mock<IClientRepository>();
        var identifiers = new Mock<IClientIdentifierRepository>();

        var client = new Client
        {
            Id = Guid.NewGuid(),
            ClientTypeId = clientTypeId,
            Cnp = "OLD_CNP",
            Cui = "OLD_CUI"
        };

        clients.Setup(x => x.GetByIdAsync(client.Id)).ReturnsAsync(client);
        uow.Setup(x => x.Clients).Returns(clients.Object);
        uow.Setup(x => x.ClientIdentifiers).Returns(identifiers.Object);
        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var service = CreateService(uow, clients, identifiersRepo: identifiers);

        await service.UpdateClientIdentifierAsync(client.Id, new UpdateClientIdentifierDto
        {
            NewIdentifier = newValue
        });

        if (clientTypeId == 1)
        {
            client.Cnp.Should().Be(newValue);
        }
        else
        {
            client.Cui.Should().Be(newValue);
        }
    }

    [Fact]
    public async Task SearchClientsAsync_ShouldSetIdentifierCorrectly()
    {
        var uow = new Mock<IUnitOfWork>();
        var clientsRepo = new Mock<IClientRepository>();

        var client = new Client
        {
            Id = Guid.NewGuid(),
            ClientTypeId = 2,
            Cui = "RO999"
        };

        clientsRepo.Setup(x => x.CountAsync(null, null)).ReturnsAsync(1);
        clientsRepo.Setup(x => x.SearchAsync(null, null, 1, 10))
            .ReturnsAsync(new List<Client> { client });

        uow.Setup(x => x.Clients).Returns(clientsRepo.Object);

        var service = CreateService(uow, clientsRepo);

        var result = await service.SearchClientsAsync(null, null, 1, 10);

        result.Items[0].Identifier.Should().Be("RO999");
    }
}
