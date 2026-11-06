# The hunt

Work in teams. The least experienced person on the team types the test while a teammate
dictates; swap for the fix. For each flaw you find, go the whole way:

1. **Hack it** — show the flaw working against your own local instance and capture
   the `PELAGOS{...}` flag it exposes.
2. **Explain it** — what the flaw is, why it works, what an attacker gets.
3. **Fix it** — change the code so the hole is closed.
4. **Prove it** — show a check that fails against the old code and passes against your fix.
   Usually that is a test in `tests/Pelagos.Tests` (the harness and a worked example, the F1a
   test, are already there; run `dotnet test` from the repo root). In the pipeline block the
   proof is the pipeline going green, and in the API block it is a Postman test. A ready-made
   reference test for every flag is linked in `tests/REFERENCE_TESTS.md`, so if yours is not coming
   together you can run that one red, then green. Never written a test? Start with `hints/writing-a-test.md`.

A captured flag is worth points. A fix with a test that proves it is worth more —
that is the part that matters. Finding the flag is the easy half.

There are twelve flags to find. Seven are core, one or two per block. Five are bonus flags,
and the trainer names them at the start of their block: capture them like any other, but fix
them only once the core flag of that block is fixed and proven. The bonus fix still scores if
you get there.

## When you are stuck

Open the matching file in `hints/` and read one tier at a time: a nudge, then the
technique, then a near-spoiler. Using a hint does not cost your fix-and-test score;
getting unstuck is the point.

## Scorecard

| # | Found (the `PELAGOS{...}` flag, or the proof) | Fixed + test |
|---|-----------------------------------------------|--------------|
| Flag 1  |  |  |
| Flag 2  |  |  |
| Flag 3  |  |  |
| Flag 4  |  |  |
| Flag 5  |  |  |
| Flag 6  |  |  |
| Flag 7  |  |  |
| Flag 8  |  |  |
| Flag 9  |  |  |
| Flag 10 |  |  |
| Flag 11 |  |  |
| Flag 12 |  |  |
