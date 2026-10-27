# Writing the test (if you have never written one)

Every test in this lab has the same shape. You do not need to learn xUnit, you need to
copy one shape and change three lines.

## The shape

```csharp
using System.Net;
using System.Net.Http.Headers;
using Xunit;

namespace Pelagos.Tests;

public class MyFlagTests : IClassFixture<ProductionAppFactory>
{
    private readonly ProductionAppFactory _factory;
    public MyFlagTests(ProductionAppFactory factory) => _factory = factory;

    [Fact]
    public async Task Say_what_must_be_true_in_plain_words()
    {
        // 1. Arrange: a client that talks to the app, in Production
        var client = _factory.CreateClient();

        // 2. Act: make the request the attacker made
        var res = await client.GetAsync("/api/the/url/you/attacked");
        var body = await res.Content.ReadAsStringAsync();

        // 3. Assert: say what a safe app does
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
        Assert.DoesNotContain("PELAGOS{", body);
    }
}
```

Put it in a new file in `tests/Pelagos.Tests`, for example `F3_Tests.cs`. The class name and
the file name can be anything. The method name is the sentence you would say to a colleague.

## The three assertions you will need

| You want to prove | Write |
|---|---|
| the app refused | `Assert.Equal(HttpStatusCode.Forbidden, res.StatusCode);` (or `NotFound`, `BadRequest`, `TooManyRequests`) |
| the app did not refuse with 200 | `Assert.NotEqual(HttpStatusCode.OK, res.StatusCode);` |
| nothing leaked in the body | `Assert.DoesNotContain("PELAGOS{", body);` |
| something is there that should be | `Assert.Contains("X-Frame-Options", res.Headers.ToString());` or `Assert.True(res.Headers.Contains("X-Frame-Options"));` |

## If the endpoint needs a login

Log in as a seeded customer first and attach the token, then make the request:

```csharp
var token = await AuthHelpers.LoginAsync(client, "customer001@pelagos.example", "0000");
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
```

To send JSON instead of a GET:

```csharp
var res = await client.PostAsJsonAsync("/api/the/url", new { field = "value" });
```

(`using System.Net.Http.Json;` at the top.)

## Red, then green

1. Write the test **before** the fix. Run `dotnet test`. It must fail. If it passes already,
   it is not testing the hole.
2. Make the fix. Run `dotnet test` again. It must pass.
3. Run only your test while you iterate:
   `dotnet test --filter "FullyQualifiedName~MyFlagTests"`

A test that was never red proves nothing. The trainer scores the fix-and-test points on
seeing both runs.

## Still stuck

Open `F1a_MisconfigTests.cs` next to your file. It is the worked example, twelve lines, and the
trainer demonstrated it on the first morning. After each hunt the trainer also hands out that
flaw's reference test: run it red, then green, and you still score.
