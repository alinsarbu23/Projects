using FluentAssertions;
using Insurance.Application.Dtos.Identity;
using Insurance.Application.DTOs.Brokers;
using Insurance.Application.DTOs.Buildings;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.DTOs.Currencies;
using Insurance.Application.DTOs.Policies;
using Insurance.IntegrationTests.Helpers;
using Insurance.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.IntegrationTests.Controllers.Policies;

public class PoliciesIntegrationTests : BaseIntegrationTest
{
    public PoliciesIntegrationTests(CustomWebApplicationFactory factory)
        : base(factory) { }

    [Fact]
    public async Task CreateDraft_And_ActivatePolicy_ShouldWork()
    {
        var adminLogin = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new { email = "admin@insurance.local", password = "Admin123!" });

        var adminAuth = await adminLogin.Content
            .ReadFromJsonAsync<AuthResponseDto>();

        Authenticate(adminAuth!.Token);

        var createCurrency = await Client.PostAsJsonAsync(
            "/api/admin/currencies",
            new CreateCurrencyDto
            {
                Code = "TEST",
                Name = "Test Currency",
                ExchangeRateToBase = 1
            });

        var currencyId = Guid.Parse(
            createCurrency.Headers.Location!.Segments.Last());

        var createBroker = await Client.PostAsJsonAsync(
            "/api/admin/brokers",
            new CreateBrokerDto
            {
                BrokerCode = "BR-INT",
                Name = "Integration Broker",
                Email = "broker@test.ro",
                Phone = "0700000000",
                CommissionPercentage = 2
            });

        var brokerId = Guid.Parse(
            createBroker.Headers.Location!.Segments.Last());

        await Client.PostAsync(
            $"/api/admin/brokers/{brokerId}/activate", null);

        await Client.PostAsJsonAsync(
            "/api/auth/register",
            new { email = "broker@test.ro", password = "Password1!" });

        var brokerLogin = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new { email = "broker@test.ro", password = "Password1!" });

        var brokerAuth = await brokerLogin.Content
            .ReadFromJsonAsync<AuthResponseDto>();

        Authenticate(brokerAuth!.Token);

        var createClient = await Client.PostAsJsonAsync(
            "/api/brokers/clients",
            new CreateClientDto
            {
                ClientTypeId = 1,
                Name = "Policy Owner",
                Cnp = "1990101223344",
                Email = "policy@test.ro",
                Phone = "0700000000",
                Address = "Bucharest"
            });

        var clientLocation = createClient.Headers.Location!;

        var createBuilding = await Client.PostAsJsonAsync(
            $"{clientLocation}/buildings",
            new CreateBuildingDto
            {
                CityId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                BuildingTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Street = "Main",
                StreetNumber = "1",
                SurfaceArea = 80,
                InsuredValue = 200000
            });

        var buildingId = Guid.Parse(
            createBuilding.Headers.Location!.Segments.Last());

        var createPolicy = await Client.PostAsJsonAsync(
            "/api/brokers/policies",
            new CreatePolicyDto
            {
                BuildingId = buildingId,
                CurrencyId = currencyId,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(30),
                BasePremium = 1000
            });

        createPolicy.StatusCode.Should().Be(HttpStatusCode.Created);

        var policyId = Guid.Parse(
            createPolicy.Headers.Location!.Segments.Last());

        var activate = await Client.PostAsync(
            $"/api/brokers/policies/{policyId}/activate", null);

        activate.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task CreatePolicy_ShouldReturn400_WhenPremiumIsNegative()
    {
        Authenticate(await AuthHelper.Login(Client));

        var response = await Client.PostAsJsonAsync(
            "/api/brokers/policies",
            new CreatePolicyDto
            {
                BuildingId = Guid.NewGuid(),
                CurrencyId = Guid.NewGuid(),
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(10),
                BasePremium = -100
            });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
