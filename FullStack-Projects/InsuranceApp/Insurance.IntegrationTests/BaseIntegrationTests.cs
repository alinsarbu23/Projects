using Insurance.IntegrationTests.Infrastructure;
using System.Net.Http.Headers;
using Xunit;

namespace Insurance.IntegrationTests;

public abstract class BaseIntegrationTest: IClassFixture<CustomWebApplicationFactory>
{
    protected readonly HttpClient Client;

    protected BaseIntegrationTest(CustomWebApplicationFactory factory)
    {
        Client = factory.CreateClient();
    }

    protected void Authenticate(string token)
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
