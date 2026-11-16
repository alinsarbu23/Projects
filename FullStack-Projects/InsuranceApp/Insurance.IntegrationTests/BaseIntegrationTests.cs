using Insurance.Application.Dtos.Identity;
using Insurance.Infrastructure;
using Insurance.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    protected readonly CustomWebApplicationFactory Factory;
    protected HttpClient Client;

    protected BaseIntegrationTest(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();

        await db.Database.EnsureDeletedAsync();
        await db.Database.MigrateAsync();

        await TestDataSeeder.SeedAsync(scope.ServiceProvider);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
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
