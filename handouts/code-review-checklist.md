# Security code review checklist

Use this on any change that touches input, data access, money, sign-in, configuration or
dependencies. Ask each question of the diff in front of you. A "no" or a "not sure" is a review
comment, not a failure.

## 1. Trust boundaries

- [ ] Where does untrusted data enter this change? Request body, query string, route, headers,
      cookies, uploaded files, messages, another service's response.
- [ ] Is every check that matters done on the server, not only in the browser or the app?

## 2. Who is calling, and what may they touch

- [ ] Does every new endpoint require a signed-in caller, unless it is deliberately public?
- [ ] For every object it reads or changes (an account, a document, a loan), is the caller checked
      against that object, not only checked as "logged in"?
- [ ] Could a role, a price, a status or an owner id be set from the request?
- [ ] Are tokens checked in full: signing key, algorithm, issuer, audience, lifetime?

## 3. Input to sink

- [ ] Follow each input to where it ends up: a SQL query, a shell command, a file path, a URL
      to redirect to or fetch, HTML, a log line.
- [ ] Is anything that gets interpreted built by gluing strings together? Bind parameters, use the
      library's safe API, or check against an allow-list.
- [ ] Is input validated at the boundary by allow-list (type, length, range, format), and rejected
      rather than repaired?
- [ ] Does the request bind only the fields a caller may change (a dedicated input type, not the
      database entity)?

## 4. Output encoding

- [ ] Is data encoded for the place it lands: HTML, an attribute, JavaScript, a URL?
- [ ] Any `Html.Raw`, `HtmlString`, `innerHTML` or HTML built from strings? Ask why.

## 5. Secrets and stored data

- [ ] No keys, tokens, passwords or connection strings in code, in committed config, or in files
      served to the browser.
- [ ] Passwords and PINs stored as salted, slow hashes, never as text, never compared as text.
- [ ] If a secret ever reached git, has it been rotated? Deleting the commit does not revoke it.

## 6. Errors and logging

- [ ] Errors return a generic problem response: no stack trace, exception text or config.
- [ ] Security events are logged: sign-ins, failures, access denied, money or data leaving.
- [ ] Nothing sensitive in the logs: secrets, tokens, PINs, full card numbers, national ids.

## 7. Dependencies and defaults

- [ ] New or upgraded packages: needed, maintained, and clean in
      `dotnet list package --vulnerable --include-transitive`?
- [ ] Did a security default change: HTTPS, CORS, cookies, headers, the developer error page,
      Swagger?

## Writing the comment

Name the risk, point at the line, suggest the fix, ask for a test. For example:

> Line 42 logs the whole request body, which includes the customer's national id. Could we log the
> request id and the outcome instead, and add a test that the log line has no id in it?

## The full reference

This page is the short version. The complete list of verifiable requirements is the
**OWASP Application Security Verification Standard (ASVS)**, version 5.0.0:
https://asvs.owasp.org/
