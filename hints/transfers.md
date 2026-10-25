# Transfers

## Nudge
You can move money from your own account. What stops you moving it from an account that is
not yours?

## Technique
The transfer endpoint does not check that you own the source account, does not check the
amount against the balance, and records nothing.

## Near-spoiler
`POST /api/transfers` with a `fromAccountId` you do not own succeeds, and the response
returns the flag.

Fix: require that the caller owns the source account, check the amount against the balance
on the server, and write an audit event for every transfer. Prove it with a test that a
transfer from an account you do not own is forbidden.
