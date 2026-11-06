# Reference tests

For each flag there is a ready-made xUnit test that proves the fix: it fails (red) against the
broken code and passes (green) once you patch the hole. This is the "prove it" step from
`HUNT.md`. The harness and a worked example (`F1a_MisconfigTests.cs`) are already in this folder.

Write your own test first; it is the part that scores and the part that teaches. Use the reference
test as a fallback if yours is not coming together, or to check your work afterwards.

**Each file name names the vulnerability.** For the most out of the exercise, open a flag's test
only after you have found and are starting to fix that flag.

## How to use one

1. Open the gist link below and click **Raw**.
2. Save each file into `tests/Pelagos.Tests/` under the same name.
3. From the repo root run `dotnet test`. It should fail against the broken code, then pass once
   your fix is in.

## Core flags

| Flag | File(s) | Link |
| --- | --- | --- |
| F1b | F1b_HeadersTests.cs | https://gist.github.com/lack3r/c4f5281ec3a8eb42d01cc6d174fb7473 |
| F2a | F2a_JwtTests.cs, AuthHelpers.Forge.cs | https://gist.github.com/lack3r/9d61a603c8acff66abc9eebc2250f788 |
| F3 | F3_SqlInjectionTests.cs | https://gist.github.com/lack3r/294679aa32198fed1a28270a576659a8 |
| F4a | F4a_IdorTests.cs | https://gist.github.com/lack3r/762a9086a4fe9377f80c256593471490 |
| F5a | F5a_FrontendSecretTests.cs | https://gist.github.com/lack3r/a5dfb5ce55cd2f0735ce2953ec832a47 |
| F8 | F8_HeistTests.cs | https://gist.github.com/lack3r/22dbec071e65b7393560ef249f43504a |

F1a has no gist; its test is already in this folder.

## Bonus flags

| Flag | File | Link |
| --- | --- | --- |
| F2b | F2b_RateLimitTests.cs | https://gist.github.com/lack3r/eaf4c0520917cd87ba921d3b8f5ef723 |
| F4b | F4b_MassAssignmentTests.cs | https://gist.github.com/lack3r/6fc419abfead2a9e3eec8b4179106053 |
| F5b | F5b_VulnerableDependencyTests.cs | https://gist.github.com/lack3r/9997ac29f850fc51f1d2814bf0df52cc |
| F6 | F6_OpenRedirectTests.cs | https://gist.github.com/lack3r/03bee83a9fdcda15b87628fdcd822af3 |
| F7 | F7_PathTraversalTests.cs | https://gist.github.com/lack3r/0a51b0500f4967c227c29f1a2be9d1e8 |
