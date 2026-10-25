namespace Pelagos.Api.Data;

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Pin { get; set; } = "0000";
}

public class Account
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Iban { get; set; } = "";
    public decimal Balance { get; set; }
}

public class BankUser
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Role { get; set; } = "teller";   // teller | analyst | admin
    public string PasswordHash { get; set; } = "";
}

public class CreditReport
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int Score { get; set; }
    public string Summary { get; set; } = "";
}

public class Statement
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Period { get; set; } = "";
    public string FilePath { get; set; } = "";
}

public class Transfer
{
    public int Id { get; set; }
    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }
    public decimal Amount { get; set; }
}

public class AuditEvent
{
    public int Id { get; set; }
    public string Action { get; set; } = "";
    public string Detail { get; set; } = "";
}
