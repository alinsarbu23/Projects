using Insurance.IntegrationTests.Infrastructure;
using System.Net.Http.Headers;
using Xunit;

public abstract class BaseIntegrationTest: IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    protected readonly HttpClient Client;

    protected BaseIntegrationTest(CustomWebApplicationFactory factory)
    {
        Client = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await DatabaseReset.ResetAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    protected void Authenticate(string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}
