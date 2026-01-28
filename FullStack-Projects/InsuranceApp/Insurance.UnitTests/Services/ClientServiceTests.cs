using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Insurance.Application.Services;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;
using Insurance.UnitTests.Helpers;

namespace Insurance.UnitTests.Services;

public class ClientServiceTests
{
    private static ClientService Create(
        Mock<IUnitOfWork> uow,
        Mock<IClientRepository> clientsRepo,
        Mock<IBuildingRepository> buildingsRepo,
        Mock<IClientIdentifierRepository> identifiersRepo,
        Mock<ILogger<ClientService>> logger)
    {
        uow.Setup(x => x.Clients).Returns(clientsRepo.Object);
        uow.Setup(x => x.Buildings).Returns(buildingsRepo.Object);
        uow.Setup(x => x.ClientIdentifiers).Returns(identifiersRepo.Object);

        return new ClientService(MapperFactory.Create(), uow.Object, logger.Object);
    }

    [Fact]
    public async Task CreateClientAsync_ShouldThrow_WhenCuiAlreadyExists()
    {
        var uow = new Mock<IUnitOfWork>();
        var clients = new Mock<IClientRepository>();
        var buildings = new Mock<IBuildingRepository>();
        var identifiers = new Mock<IClientIdentifierRepository>();
        var logger = new Mock<ILogger<ClientService>>();

        clients.Setup(x => x.ExistsByCuiAsync("RO1")).ReturnsAsync(true);

        var service = Create(uow, clients, buildings, identifiers, logger);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await service.CreateClientAsync(new CreateClientDto
            {
                ClientType = ClientType.Company,
                Name = "Comp",
                Cui = "RO1",
                Email = "a@a.ro",
                Phone = "0700",
                Address = "Addr"
            });
        });
    }

    [Fact]
    public async Task CreateClientAsync_ShouldLogWarning_AndThrow_WhenSaveChangesIsZero()
    {
        var uow = new Mock<IUnitOfWork>();
        var clients = new Mock<IClientRepository>();
        var buildings = new Mock<IBuildingRepository>();
        var identifiers = new Mock<IClientIdentifierRepository>();
        var logger = new Mock<ILogger<ClientService>>();

        clients.Setup(x => x.ExistsByCuiAsync(It.IsAny<string>())).ReturnsAsync(false);
        clients.Setup(x => x.ExistsByCnpAsync(It.IsAny<string>())).ReturnsAsync(false);
        clients.Setup(x => x.AddAsync(It.IsAny<Client>())).Returns(Task.CompletedTask);

        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

        var service = Create(uow, clients, buildings, identifiers, logger);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await service.CreateClientAsync(new CreateClientDto
            {
                ClientType = ClientType.Company,
                Name = "Comp",
                Cui = "RO1",
                Email = "a@a.ro",
                Phone = "0700",
                Address = "Addr"
            });
        });

        logger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Failed to create client")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetClientByIdAsync_ShouldPopulate_ClientType_Identifier_AndBuildings()
    {
        var uow = new Mock<IUnitOfWork>();
        var clients = new Mock<IClientRepository>();
        var buildings = new Mock<IBuildingRepository>();
        var identifiers = new Mock<IClientIdentifierRepository>();
        var logger = new Mock<ILogger<ClientService>>();

        var clientId = Guid.NewGuid();

        clients.Setup(x => x.GetByIdAsync(clientId)).ReturnsAsync(new Client
        {
            Id = clientId,
            ClientType = ClientType.Individual,
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

        var service = Create(uow, clients, buildings, identifiers, logger);

        var dto = await service.GetClientByIdAsync(clientId);

        dto.Id.Should().Be(clientId);
        dto.ClientType.Should().Be(ClientType.Individual.ToString());
        dto.Identifier.Should().Be("123");
        dto.Buildings.Should().NotBeNull();
        dto.Buildings.Count.Should().Be(2);
    }

    [Fact]
    public async Task UpdateClientAsync_ShouldThrow_WhenSaveChangesIsZero()
    {
        var uow = new Mock<IUnitOfWork>();
        var clients = new Mock<IClientRepository>();
        var buildings = new Mock<IBuildingRepository>();
        var identifiers = new Mock<IClientIdentifierRepository>();
        var logger = new Mock<ILogger<ClientService>>();

        var clientId = Guid.NewGuid();

        clients.Setup(x => x.GetByIdAsync(clientId))
            .ReturnsAsync(new Client { Id = clientId });

        clients.Setup(x => x.UpdateAsync(It.IsAny<Client>()))
            .Returns(Task.CompletedTask);

        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

        var service = Create(uow, clients, buildings, identifiers, logger);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await service.UpdateClientAsync(clientId, new UpdateClientDto
            {
                Name = "Test",
                Email = "a@a.ro",
                Phone = "0700",
                Address = "Addr"
            });
        });
    }

    [Theory]
    [InlineData(ClientType.Individual, "NEW_CNP")]
    [InlineData(ClientType.Company, "NEW_CUI")]
    public async Task UpdateClientIdentifierAsync_ShouldUpdateCorrectField(
    ClientType type,
    string newValue)
    {
        var uow = new Mock<IUnitOfWork>();
        var clients = new Mock<IClientRepository>();
        var identifiers = new Mock<IClientIdentifierRepository>();

        var client = new Client
        {
            Id = Guid.NewGuid(),
            ClientType = type,
            Cnp = "OLD_CNP",
            Cui = "OLD_CUI"
        };

        clients.Setup(x => x.GetByIdAsync(client.Id)).ReturnsAsync(client);
        identifiers.Setup(x => x.GetLastChangeDateAsync(client.Id)).ReturnsAsync((DateTime?)null);

        uow.Setup(x => x.Clients).Returns(clients.Object);
        uow.Setup(x => x.ClientIdentifiers).Returns(identifiers.Object);
        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var service = new ClientService(MapperFactory.Create(), uow.Object, Mock.Of<ILogger<ClientService>>());

        await service.UpdateClientIdentifierAsync(client.Id, new UpdateClientIdentifierDto
        {
            NewIdentifier = newValue
        });

        if (type == ClientType.Individual)
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
            ClientType = ClientType.Company,
            Cui = "RO999"
        };

        clientsRepo.Setup(x => x.CountAsync(null, null)).ReturnsAsync(1);
        clientsRepo.Setup(x => x.SearchAsync(null, null, 1, 10))
            .ReturnsAsync(new List<Client> { client });

        uow.Setup(x => x.Clients).Returns(clientsRepo.Object);

        var service = new ClientService(MapperFactory.Create(), uow.Object, Mock.Of<ILogger<ClientService>>());

        var result = await service.SearchClientsAsync(null, null, 1, 10);

        result.Items[0].Identifier.Should().Be("RO999");
    }
}
