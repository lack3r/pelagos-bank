# Secrets and packages

## Nudge
What does the browser download to run the portal? And do you trust every package the
project pulls in, including the ones you did not add yourself?

## Technique
A secret is shipped inside the front-end script, readable by anyone who views the page
source. And one dependency has a published security advisory.

## Near-spoiler
- Open `wwwroot/app.js` (view source in the browser): a constant holds the flag.
- Run `dotnet list package --vulnerable --include-transitive` from the project: it names
  a package with a known advisory.

Fix: remove the secret from the bundle (keep it server-side and rotate the leaked value),
and upgrade the flagged package until the audit reports no vulnerable packages. Prove both
with tests.
