using System.Net;

namespace Pelagos.Tests;

public class HealthTests : IClassFixture<ProductionAppFactory>
{
    private readonly ProductionAppFactory _factory;
    public HealthTests(ProductionAppFactory factory) => _factory = factory;

    [Fact]
    public async Task Health_returns_ok()
    {
        var client = _factory.CreateClient();
        var res = await client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }
}
