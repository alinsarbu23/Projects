using Insurance.Application.Dtos.Identity;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Insurance.IntegrationTests;

public static class TestAuth
{
    public static async Task LoginAsBroker(HttpClient client)
    {
        var login = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "broker@test.ro",
            password = "test"
        });

        var auth = await login.Content.ReadFromJsonAsync<AuthResponseDto>();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);
    }
}
