# Customer search

## Nudge
The search builds a database query from the name you type. What if the name is not just
a name?

## Technique
The query puts your input straight into raw SQL, so your input can change what the query
means, not just what it matches.

## Near-spoiler
`GET /api/customers/search?name=` runs your input as part of the SQL WHERE clause. An
input that makes the match always true returns every customer, including a hidden one
whose email field holds the flag.

Fix: parameterise the query (an EF Core `FromSql` with a parameter, or LINQ), so the
input is only ever data. Prove it with a test that a crafted input returns no extra rows.
