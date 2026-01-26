using Insurance.Application.Dtos.Identity;
using Insurance.Application.DTOs.Identity;
using System.Net.Http.Json;

namespace Insurance.IntegrationTests.Helpers;

public static class AuthHelper
{
    public static async Task<string> LoginAsBroker(HttpClient client)
    {
        await client.PostAsJsonAsync("/api/auth/register", new RegisterDto
        {
            Email = "broker@test.ro",
            Password = "Password1!",
            Role = "Broker"
        });

        var response = await client.PostAsJsonAsync("/api/auth/login", new LoginDto
        {
            Email = "broker@test.ro",
            Password = "Password1!"
        });

        var auth = await response.Content
            .ReadFromJsonAsync<AuthResponseDto>();

        return auth!.Token;
    }
}
