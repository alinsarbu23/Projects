using FluentAssertions;
using Insurance.Application.DTOs.Clients;
using Insurance.IntegrationTests.Helpers;
using Insurance.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.IntegrationTests.Controllers.Clients;

public class ClientsIntegrationTests : BaseIntegrationTest
{
    public ClientsIntegrationTests(CustomWebApplicationFactory factory)
        : base(factory) { }

    [Fact]
    public async Task Create_And_Get_Client_ShouldWork()
    {
        Authenticate(await AuthHelper.Login(Client));

        var create = await Client.PostAsJsonAsync(
            "/api/brokers/clients",
            new CreateClientDto
            {
                ClientTypeId = 1,
                Name = "Ion",
                Cnp = "1900101223344",
                Email = "ion@test.ro",
                Phone = "0700",
                Address = "Bucuresti"
            });


        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var get = await Client.GetAsync(create.Headers.Location!);
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var client = await get.Content.ReadFromJsonAsync<GetClientByIdDto>();

        client.Should().NotBeNull();
        client!.Name.Should().Be("Ion");
        client.Email.Should().Be("ion@test.ro");
        client.Phone.Should().Be("0700");
        client.Address.Should().Be("Bucuresti");
        client.Identifier.Should().Be("1900101223344");
    }

    [Fact]
    public async Task CreateClient_ShouldReturn400_WhenInvalid()
    {
        Authenticate(await AuthHelper.Login(Client));

        var response = await Client.PostAsJsonAsync(
            "/api/brokers/clients", new { });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
