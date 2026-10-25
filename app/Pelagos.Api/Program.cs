using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Pelagos.Api.Data;
using Pelagos.Api.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PelagosDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Pelagos") ?? "Data Source=pelagos.db"));

var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            SignatureValidator = (token, _) => new JsonWebToken(token)
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("login", o =>
    {
        o.Window = TimeSpan.FromMinutes(1);
        o.PermitLimit = 5;
        o.QueueLimit = 0;
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PelagosDbContext>();
    db.Database.EnsureCreated();
    Seed.Run(db);
}

app.UseDeveloperExceptionPage();
app.UseStaticFiles();

app.Use(async (ctx, next) =>
{
    ctx.Response.Headers["X-Pelagos-Build"] = "PELAGOS{verbose_headers_leak_build}";
    await next();
});

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/api/reports/{id:int}", (int id, PelagosDbContext db, IConfiguration config) =>
{
    if (id == 0)
        throw new InvalidOperationException(
            $"credit engine offline; apiKey={config["CreditBureau:ApiKey"]}");
    var report = db.CreditReports.Find(id);
    return report is null ? Results.NotFound() : Results.Ok(report);
});

app.MapPost("/api/auth/login", (LoginRequest req, PelagosDbContext db, IConfiguration config) =>
{
    string role;
    string subject;

    var staff = db.BankUsers.FirstOrDefault(u => u.Username == req.Username);
    if (staff is not null && staff.PasswordHash == req.Password)
    {
        role = staff.Role;
        subject = $"staff:{staff.Id}";
    }
    else
    {
        var customer = db.Customers.FirstOrDefault(c => c.Email == req.Username);
        if (customer is null || customer.Pin != req.Password)
            return Results.Json(new { error = "invalid", hint = "PELAGOS{login_brute_force_unthrottled}" },
                statusCode: StatusCodes.Status401Unauthorized);
        role = "customer";
        subject = customer.Id.ToString();
    }

    var jwt = config.GetSection("Jwt");
    var creds = new SigningCredentials(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SigningKey"]!)),
        SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: jwt["Issuer"],
        audience: jwt["Audience"],
        claims: new[]
        {
            new Claim(ClaimTypes.NameIdentifier, subject),
            new Claim(ClaimTypes.Role, role)
        },
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: creds);
    return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
});

app.MapGet("/api/accounts/{id:int}", (int id, PelagosDbContext db, ClaimsPrincipal user) =>
{
    var account = db.Accounts.Find(id);
    if (account is null) return Results.NotFound();

    return Results.Ok(account);
}).RequireAuthorization();

app.MapGet("/api/customers/search", (string name, PelagosDbContext db) =>
{
    var matches = db.Customers
        .FromSqlRaw("SELECT * FROM Customers WHERE FullName LIKE '%" + name + "%'")
        .ToList();
    return Results.Ok(matches);
}).RequireAuthorization();

app.MapGet("/api/admin/flag", (IConfiguration config) =>
    Results.Ok(new { flag = config["Flags:Admin"] }))
   .RequireAuthorization(policy => policy.RequireRole("admin"));

app.MapPut("/api/profile", (BankUser update, PelagosDbContext db, ClaimsPrincipal user) =>
{
    var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
    if (id is null || !id.StartsWith("staff:")) return Results.Forbid();
    var staff = db.BankUsers.Find(int.Parse(id["staff:".Length..]));
    if (staff is null) return Results.NotFound();

    staff.Username = update.Username;
    staff.Role = update.Role;
    db.SaveChanges();
    return Results.NoContent();
}).RequireAuthorization();

app.MapGet("/api/statements", (string name, IWebHostEnvironment env) =>
{
    var root = Path.Combine(env.ContentRootPath, "statements");
    var full = Path.Combine(root, name);
    return File.Exists(full) ? Results.File(full, "application/octet-stream") : Results.NotFound();
}).RequireAuthorization();

app.MapPost("/api/transfers", (Transfer req, PelagosDbContext db, IConfiguration config, ClaimsPrincipal user) =>
{
    var from = db.Accounts.Find(req.FromAccountId);
    if (from is null) return Results.NotFound();

    from.Balance -= req.Amount;
    db.Transfers.Add(new Transfer { FromAccountId = req.FromAccountId, ToAccountId = req.ToAccountId, Amount = req.Amount });
    db.SaveChanges();
    return Results.Ok(new { ok = true, note = config["Flags:Transfer"] });
}).RequireAuthorization();

app.MapGet("/api/continue", (string returnUrl) =>
    Results.Redirect(returnUrl));

app.Run();

public partial class Program { }   // expose for WebApplicationFactory
