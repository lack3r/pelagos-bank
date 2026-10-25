namespace Pelagos.Api.Data;

public static class Seed
{
    public static void Run(PelagosDbContext db)
    {
        if (db.Customers.Any()) return;

        for (var i = 1; i <= 40; i++)
        {
            db.Customers.Add(new Customer { Id = i, FullName = $"Customer {i:D3}", Email = $"customer{i:D3}@pelagos.example", Pin = "0000" });
            db.Accounts.Add(new Account { Id = i, CustomerId = i, Iban = $"CY00PELA0000000000000{i:D5}", Balance = 1000m + i * 25 });
            db.CreditReports.Add(new CreditReport { Id = i, CustomerId = i, Score = 300 + (i * 7 % 550), Summary = $"Standard risk profile for customer {i:D3}." });
            db.Statements.Add(new Statement { Id = i, AccountId = i, Period = "2026-10", FilePath = $"statements/{i:D3}_2026-10.pdf" });
        }

        // Internal, non-listed customer record.
        db.Customers.Add(new Customer
        {
            Id = 999,
            FullName = "ZZZ INTERNAL — DO NOT DISPLAY",
            Email = "PELAGOS{sql_injection_dumps_hidden_rows}",
            Pin = "0000"
        });
        // High-value account held by customer 7.
        db.Accounts.Add(new Account
        {
            Id = 700,
            CustomerId = 7,
            Iban = "PELAGOS{idor_reads_other_accounts}",
            Balance = 999999m
        });

        db.BankUsers.AddRange(
            new BankUser { Id = 1, Username = "teller1", Role = "teller", PasswordHash = "seed" },
            new BankUser { Id = 2, Username = "analyst1", Role = "analyst", PasswordHash = "seed" },
            new BankUser { Id = 3, Username = "admin", Role = "admin", PasswordHash = "seed" });

        db.SaveChanges();
    }
}
