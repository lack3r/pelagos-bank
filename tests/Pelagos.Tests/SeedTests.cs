using Microsoft.EntityFrameworkCore;
using Pelagos.Api.Data;
using Xunit;

namespace Pelagos.Tests;

public class SeedTests
{
    [Fact]
    public void Seed_populates_core_tables()
    {
        var opts = new DbContextOptionsBuilder<PelagosDbContext>()
            .UseSqlite($"Data Source=seedtest_{Guid.NewGuid():N}.db").Options;
        using var db = new PelagosDbContext(opts);
        db.Database.EnsureCreated();
        Seed.Run(db);

        Assert.True(db.Customers.Count() >= 40);
        Assert.True(db.CreditReports.Count() >= 40);
        Assert.True(db.BankUsers.Count() >= 3);
    }
}
