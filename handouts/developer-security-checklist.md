# Developer security checklist

One page to keep next to your editor. Tick what your service does today; the gaps are your backlog.
Written for ASP.NET Core, but every line has an equivalent in any stack.

## Before you write code

- [ ] You know which data in this service is sensitive (personal, financial, credentials) and where it flows
- [ ] New features get a five-minute threat model: who could misuse this, and how (STRIDE is enough)
- [ ] Abuse cases sit next to the user stories ("a customer tries to read someone else's statement")

## In the code

- [ ] **Input:** validated on the server against an allow-list (type, length, format), never trusted from the client
- [ ] **Queries:** parameters only, never a string built from input (EF Core LINQ, or `FromSql` with interpolation)
- [ ] **Output:** encoded for where it lands (HTML, JavaScript, URL); no `Html.Raw` or `innerHTML` with data
- [ ] **Authentication:** every token fully validated (signature, issuer, audience, lifetime, allowed algorithms)
- [ ] **Authorization:** deny by default (a fallback policy that requires a signed-in user); every object
      checked against the caller, not just the route
- [ ] **Binding:** requests bind to DTOs that list only the fields a caller may set
- [ ] **Limits:** rate limiting on logins, resets, one-time codes and anything expensive
- [ ] **Passwords:** hashed with a slow, salted algorithm (`PasswordHasher<T>`, PBKDF2, Argon2), never stored or compared in plain text
- [ ] **Secrets:** from `dotnet user-secrets` in development and a vault or the environment in production;
      none in source, in committed config, or in anything served to the browser
- [ ] **Files and redirects:** any file name or return URL from a request is untrusted input
- [ ] **Errors:** the developer exception page only in Development; `UseExceptionHandler` and ProblemDetails
      everywhere else, no stack traces or config values in responses
- [ ] **Logging:** security events logged (sign-ins, failures, access denied, money or data movement);
      never secrets, tokens, passwords, PINs or full card numbers

## In the browser

- [ ] HTTPS redirection and HSTS outside Development
- [ ] Headers: `X-Content-Type-Options: nosniff`, `frame-ancestors` or `X-Frame-Options`, `Referrer-Policy`,
      a Content-Security-Policy; no header that announces your build or version
- [ ] CORS lists named origins only; never "any origin" together with credentials
- [ ] Cookies `HttpOnly`, `Secure`, `SameSite`; antiforgery tokens on cookie-authenticated forms

## Before you merge

- [ ] A security reviewer's question answered for each change: what can a hostile caller do with this?
- [ ] Every security fix ships with a test that fails without it
- [ ] Build warnings read, not scrolled past (analyzers such as EF1003, NuGet audit NU1901 to NU1904)

## In the pipeline

- [ ] Restore, build and tests on every pull request; a red gate blocks the merge
- [ ] Dependency audit (`dotnet list package --vulnerable --include-transitive`, Dependabot); high and
      critical advisories fail the build
- [ ] Secret scan over the full git history, with rules tuned to how your team names things
- [ ] Static analysis (CodeQL, Semgrep) on every pull request; a custom rule for every class of bug you fix
- [ ] A dynamic baseline scan (OWASP ZAP) against a test deployment before release

## When something goes wrong

- [ ] A leaked secret is rotated at once; deleting the commit is not a fix
- [ ] You know who to call in the first hour, and the plan says who decides what
- [ ] After the fix: a blameless review, a regression test, and a rule so the class does not come back
