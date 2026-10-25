using System.Net;
using Xunit;

namespace Pelagos.Tests;

public class F1a_MisconfigTests : IClassFixture<ProductionAppFactory>
{
    private readonly ProductionAppFactory _factory;
    public F1a_MisconfigTests(ProductionAppFactory factory) => _factory = factory;

    [Fact]
    [Trait("Flag", "F1a")]
    public async Task Unhandled_error_in_production_hides_internals()
    {
        var client = _factory.CreateClient();
        var res = await client.GetAsync("/api/reports/0");

        Assert.Equal(HttpStatusCode.InternalServerError, res.StatusCode);
        var body = await res.Content.ReadAsStringAsync();
        Assert.DoesNotContain("PELAGOS{", body);   // no leaked flag/secret
        Assert.DoesNotContain("   at ", body);      // no stack frames
        Assert.DoesNotContain("credit engine offline", body);   // no raw exception message
    }

    [Fact]
    [Trait("Flag", "F1a")]
    public async Task Swagger_is_not_served_in_production()
    {
        var client = _factory.CreateClient();
        var res = await client.GetAsync("/swagger/index.html");
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }
}
