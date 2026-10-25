# Logging in and tokens

## Nudge
How carefully does the API check your bearer token? And how many times can you get the
login wrong before anything stops you?

## Technique
A JSON Web Token is only safe if the server validates its signature, algorithm, issuer,
audience and expiry. And a login with no throttle invites guessing, especially against
a short PIN.

## Near-spoiler
- The bearer handler does not enforce the signing key or algorithm, so a token the
  server never issued (signed with the wrong key, or with `alg` set to none) carrying a
  `role` of admin reaches `GET /api/admin/flag`.
- `POST /api/auth/login` has no rate limit, so a four-digit PIN can be tried endlessly.

Fix: enforce the token validation parameters (signing key, issuer, audience, lifetime,
and an allowed-algorithms list); add a fixed-window limiter (5 a minute, 429 after)
required on the login endpoint. Prove both with tests.
