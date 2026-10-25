using System.Net;
using System.Net.Http.Headers;
using Xunit;

namespace Pelagos.Tests;

public class AuthInfraTests : IClassFixture<ProductionAppFactory>
{
    private readonly ProductionAppFactory _factory;
    public AuthInfraTests(ProductionAppFactory factory) => _factory = factory;

    [Fact]
    public async Task Customer_can_log_in_and_read_their_own_account()
    {
        var client = _factory.CreateClient();
        var token = await AuthHelpers.LoginAsync(client, "customer007@pelagos.example", "0000");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var res = await client.GetAsync("/api/accounts/7");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task Protected_endpoint_rejects_anonymous()
    {
        var client = _factory.CreateClient();
        var res = await client.GetAsync("/api/accounts/7");
        Assert.Equal(HttpStatusCode.Unauthorized, res.StatusCode);
    }

    [Fact]
    public async Task Non_admin_cannot_reach_admin_flag()
    {
        var client = _factory.CreateClient();
        var token = await AuthHelpers.LoginAsync(client, "customer001@pelagos.example", "0000");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var res = await client.GetAsync("/api/admin/flag");
        Assert.Equal(HttpStatusCode.Forbidden, res.StatusCode);
    }
}
