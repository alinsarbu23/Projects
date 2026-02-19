using Insurance.Application.Dtos.Identity;
using Insurance.IntegrationTests.Infrastructure;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly CustomWebApplicationFactory Factory;
    protected HttpClient Client;

    protected BaseIntegrationTest(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    protected void Authenticate(string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    protected void ClearAuthentication()
    {
        Client.DefaultRequestHeaders.Authorization = null;
    }

    protected async Task LoginAsAdmin()
    {
        var login = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new { email = "admin@insurance.local", password = "Admin123!" });

        var auth = await login.Content.ReadFromJsonAsync<AuthResponseDto>();
        Authenticate(auth!.Token);
    }
}
