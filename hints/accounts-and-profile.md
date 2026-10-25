# Accounts and profile

## Nudge
You can read your own account and edit your own profile. What decides which account you
see, and which fields the update accepts?

## Technique
One endpoint returns a record by id without checking that it is yours. Another binds the
whole request body, including a field you should not be able to set.

## Near-spoiler
- `GET /api/accounts/{id}` returns any account by id with no ownership check. Read the
  high-value account that is not yours: its IBAN field is the flag.
- `PUT /api/profile` binds a role from the request body, so you can promote yourself.

Fix: compare the caller's id claim to the account's owner (else return 403); on the
update, bind and set only the display name, never the role. Prove both with tests.
