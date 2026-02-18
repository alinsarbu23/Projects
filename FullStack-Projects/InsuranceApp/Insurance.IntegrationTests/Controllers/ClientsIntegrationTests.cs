using FluentAssertions;
using Insurance.Application.Dtos.Identity;
using Insurance.Application.DTOs.Clients;
using Insurance.IntegrationTests.Helpers;
using Insurance.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.IntegrationTests.Controllers;

public class ClientsIntegrationTests : BaseIntegrationTest
{
    public ClientsIntegrationTests(CustomWebApplicationFactory factory)
        : base(factory) { }

    [Fact]
    public async Task Create_And_Get_Client_ShouldWork()
    {
        var adminLogin = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new { email = "admin@insurance.local", password = "Admin123!" });

        var adminAuth = await adminLogin.Content.ReadFromJsonAsync<AuthResponseDto>();
        Authenticate(adminAuth!.Token);

        var createBroker = await Client.PostAsJsonAsync(
            "/api/brokers",
            new
            {
                brokerCode = "BR-CLIENT",
                name = "Client Broker",
                email = "clientbroker@test.ro",
                phone = "0700000000",
                commissionPercentage = 2m
            });

        var brokerId = Guid.Parse(createBroker.Headers.Location!.Segments.Last());

        await Client.PostAsync($"/api/brokers/{brokerId}/activate", null);

        await Client.PostAsJsonAsync(
            "/api/auth/register",
            new { email = "clientbroker@test.ro", password = "Password1!" });

        var brokerLogin = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new { email = "clientbroker@test.ro", password = "Password1!" });

        var brokerAuth = await brokerLogin.Content.ReadFromJsonAsync<AuthResponseDto>();
        Authenticate(brokerAuth!.Token);

        var create = await Client.PostAsJsonAsync(
            "/api/clients",
            new CreateClientDto
            {
                ClientTypeId = 1,
                Name = "Ion",
                Cnp = "1900101223344",
                Email = "ion@test.ro",
                Phone = "0700000000",
                Address = "Bucuresti"
            });

        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var get = await Client.GetAsync(create.Headers.Location!);
        get.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateClient_ShouldReturn400_WhenInvalid()
    {
        Authenticate(await AuthHelper.Login(Client));

        var response = await Client.PostAsJsonAsync(
            "/api/clients", new { });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
