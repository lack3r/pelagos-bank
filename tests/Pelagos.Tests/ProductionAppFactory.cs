using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Pelagos.Tests;

public class ProductionAppFactory : WebApplicationFactory<Program>
{
    private readonly string _dbFile = $"test_{Guid.NewGuid():N}.db";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Production");
        // Pooling=False: each test owns its file and releases it on close, so no global
        // SqliteConnection.ClearAllPools() is needed — that call is process-wide and would
        // yank pooled connections out from under other test classes' factories running in
        // parallel, causing intermittent host-startup failures.
        builder.UseSetting("ConnectionStrings:Pelagos", $"Data Source={_dbFile};Pooling=False");
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);   // tear down host + DbContext first, release the file
        if (!disposing) return;

        foreach (var suffix in new[] { "", "-shm", "-wal", "-journal" })
        {
            try { File.Delete(_dbFile + suffix); } catch { /* best effort */ }
        }
    }
}
