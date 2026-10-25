using Microsoft.EntityFrameworkCore;

namespace Pelagos.Api.Data;

public class PelagosDbContext(DbContextOptions<PelagosDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<BankUser> BankUsers => Set<BankUser>();
    public DbSet<CreditReport> CreditReports => Set<CreditReport>();
    public DbSet<Statement> Statements => Set<Statement>();
    public DbSet<Transfer> Transfers => Set<Transfer>();
    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();
}
