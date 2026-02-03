using Insurance.IntegrationTests.Infrastructure;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

public abstract class BaseIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    protected HttpClient Client { get; }

    protected BaseIntegrationTest(CustomWebApplicationFactory factory)
    {
        Client = factory.CreateClient();
    }

    protected void Authenticate(string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}
