# Error responses and headers

## Nudge
What does the API do when a request goes wrong? And look at the response headers, not
just the body.

## Technique
In Production a developer error page should never render; if it does, an unhandled
error leaks internal detail. Separately, a few defensive response headers should be on
every response and are not.

## Near-spoiler
- `GET /api/reports/0` throws an unhandled error, and the error page prints the
  exception message, which contains a configuration value. That value is a flag.
- Check any response for `X-Content-Type-Options`, `X-Frame-Options` and
  `Referrer-Policy`: all three are missing.

Fix: replace the developer page with a problem-details handler, gate Swagger to the
Development environment, add a middleware that sets the three headers, and move the
secret out of config and rotate it. Prove each with a test.
