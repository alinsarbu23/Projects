using Xunit;
using Moq;
using FluentAssertions;
using Insurance.Api.Controllers;
using Insurance.Application.Interfaces.Services;
using Insurance.Application.DTOs.Clients;
using Insurance.Domain.Enums;

namespace Insurance.UnitTests.Controllers;

public class ClientsControllerTests
{
    private readonly Mock<IClientService> _clientServiceMock;
    private readonly ClientsController _controller;

    public ClientsControllerTests()
    {
        _clientServiceMock = new Mock<IClientService>();
        _controller = new ClientsController(_clientServiceMock.Object);
    }

    [Fact]
    public async Task CreateClient_ShouldCallService()
    {
        _clientServiceMock
            .Setup(x => x.CreateClientAsync(It.IsAny<CreateClientDto>()))
            .ReturnsAsync(Guid.NewGuid());

        var result = await _controller.CreateClient(new CreateClientDto
        {
            ClientType = ClientType.Individual,
            Name = "Test",
            Cnp = "123",
            Email = "test@test.ro",
            Phone = "0700",
            Address = "Addr"
        });

        result.Should().NotBeNull();

        _clientServiceMock.Verify(x => x.CreateClientAsync(It.IsAny<CreateClientDto>()), Times.Once);
    }

    [Fact]
    public async Task UpdateClient_ShouldCallService()
    {
        var clientId = Guid.NewGuid();

        var result = await _controller.UpdateClient(
            clientId,
            new UpdateClientDto
            {
                Email = "a@a.ro",
                Phone = "0700",
                Address = "Addr"
            });

        result.Should().NotBeNull();

        _clientServiceMock.Verify(x => x.UpdateClientAsync(clientId, It.IsAny<UpdateClientDto>()), Times.Once);
    }

    [Fact]
    public async Task UpdateClientIdentifier_ShouldCallService()
    {
        var clientId = Guid.NewGuid();

        var result = await _controller.UpdateClientIdentifier(
            clientId,
            new UpdateClientIdentifierDto
            {
                NewIdentifier = "NEW123"
            });

        result.Should().NotBeNull();

        _clientServiceMock.Verify(
            x => x.UpdateClientIdentifierAsync(
                clientId,
                It.IsAny<UpdateClientIdentifierDto>()),
            Times.Once);
    }

    [Fact]
    public async Task GetClientById_ShouldReturnResult()
    {
        _clientServiceMock
            .Setup(x => x.GetClientByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new GetClientByIdDto
            {
                Id = Guid.NewGuid(),
                Name = "Client",
                ClientType = "Individual",
                Identifier = "123",
                Email = "a@a.ro",
                Phone = "0700",
                Address = "Addr",
                Buildings = []
            });

        var result = await _controller.GetClientById(Guid.NewGuid());

        result.Should().NotBeNull();

        _clientServiceMock.Verify(x => x.GetClientByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task SearchClients_ShouldCallService()
    {
        _clientServiceMock
            .Setup(x => x.SearchClientsAsync(null, null, 1, 10))
            .ReturnsAsync(new PagedClientsDto
            {
                Page = 1,
                PageSize = 10,
                TotalCount = 0,
                Items = []
            });

        var result = await _controller.SearchClients(null, null);

        result.Should().NotBeNull();

        _clientServiceMock.Verify(x => x.SearchClientsAsync(null, null, 1, 10), Times.Once);
    }
}
