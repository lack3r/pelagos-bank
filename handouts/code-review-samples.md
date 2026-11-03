# Code review samples

Four short pull requests from an imaginary lending app. Each one compiles and works for the
happy path. Review them as a team with the checklist (`code-review-checklist.md`).

For each sample, write a review comment of one or two lines:

1. What is the risk, in plain words?
2. Which line causes it?
3. What would you ask the author to change, and what test would prove it?

Start with your team's number, then do the others. Keep it to about four minutes each.

---

## Sample 1: a campaign click tracker

A marketing email links to `/track/{campaignId}?to=...`. The endpoint counts the click, then sends
the reader on to the landing page.

```csharp
app.MapGet("/track/{campaignId:int}", async (int campaignId, string to,
    MarketingDb db) =>
{
    db.Clicks.Add(new Click(campaignId, DateTime.UtcNow));
    await db.SaveChangesAsync();
    return Results.Redirect(to);
});
```

- Who decides where this endpoint sends the browser?
- What could someone do with a link that starts with your bank's own domain?
- Where should the destination come from instead?

---

## Sample 2: download an export

Signed-in users download the CSV exports they ran earlier. Exports are stored per year.

```csharp
app.MapGet("/exports/{year}/{fileName}",
    (string year, string fileName, IConfiguration cfg) =>
{
    var dir = cfg["Exports:Folder"];
    var bytes = File.ReadAllBytes($"{dir}/{year}/{fileName}");
    return Results.File(bytes, "text/csv", fileName);
}).RequireAuthorization();
```

- What do `year` and `fileName` really control?
- Which files on the server could a signed-in user read? Whose exports could they read?
- There are two ways to fix this. Which one is stronger, and why?

---

## Sample 3: edit a loan application

Customers can edit a loan application while it is still a draft.

```csharp
public class LoanApplication
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string Purpose { get; set; } = "";
    public string Status { get; set; } = "Draft";
    public decimal InterestRate { get; set; }
}

app.MapPut("/loans/{id:int}", async (int id, LoanApplication input,
    LoanDb db) =>
{
    input.Id = id;
    db.Update(input);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();
```

- Which fields can the caller set? Which ones should a customer never set?
- What happens if the body also carries `"Status": "Approved"` or `"InterestRate": 0`?
- What else is missing before this update is allowed?

---

## Sample 4: request logging

A developer added this middleware to debug a production issue, and it stayed.

```csharp
app.Use(async (ctx, next) =>
{
    var auth = ctx.Request.Headers.Authorization.ToString();
    ctx.Request.EnableBuffering();
    var body = await new StreamReader(ctx.Request.Body).ReadToEndAsync();
    ctx.Request.Body.Position = 0;

    app.Logger.LogInformation("{Method} {Path} auth={Auth} body={Body}",
        ctx.Request.Method, ctx.Request.Path, auth, body);
    await next();
});
```

- What ends up in the log when a customer signs in, or applies for a loan?
- Who can read these logs, where are they shipped, and how long are they kept?
- What should this middleware log instead?
