using System.Net.Http.Json;

namespace Pelagos.Tests;

public static class AuthHelpers
{
    public static async Task<string> LoginAsync(HttpClient client, string username, string password)
    {
        var res = await client.PostAsJsonAsync("/api/auth/login", new { username, password });
        res.EnsureSuccessStatusCode();
        var body = await res.Content.ReadFromJsonAsync<TokenResponse>();
        return body!.Token;
    }

    private record TokenResponse(string Token);
}
