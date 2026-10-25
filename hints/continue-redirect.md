# The continue / redirect endpoint

## Nudge
The continue endpoint sends you somewhere after an action. Who decides where it sends you?

## Technique
It redirects to whatever URL you hand it, with no check that the destination is a page on
this site.

## Near-spoiler
`GET /api/continue?returnUrl=https://example.com/` redirects the browser off-site. A link
that starts on the bank's real domain and lands on a copy is the whole danger.

Fix: allow only a local path (it must start with a single `/`, and reject the `//` and
`/\` tricks that smuggle an absolute URL through), otherwise send the user home. Prove it
with a test that an off-site URL is not followed.
