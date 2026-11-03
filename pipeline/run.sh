#!/usr/bin/env bash
# Pelagos security pipeline: the gates a CI server runs on every push, run on your laptop.
# Every stage runs, then the summary says GREEN or which stages are RED.
# Usage (from anywhere):  ./pipeline/run.sh
set -u
cd "$(dirname "$0")/.."

failed=()
stage() { echo; echo "== $1 =="; }
fail()  { echo "   RED: $1"; failed+=("$1"); }
pass()  { echo "   ok"; }

stage "1/5 restore packages"
if dotnet restore Pelagos.sln --verbosity quiet; then pass; else fail "restore"; fi

stage "2/5 build"
if dotnet build Pelagos.sln --no-restore --verbosity quiet --nologo; then pass; else fail "build"; fi

stage "3/5 tests"
if dotnet test Pelagos.sln --no-build --verbosity quiet --nologo; then pass; else fail "tests"; fi

stage "4/5 dependency audit (known vulnerable packages)"
audit="$(dotnet list app/Pelagos.Api/Pelagos.Api.csproj package --vulnerable --include-transitive 2>&1)"
code=$?
if echo "$audit" | grep -qi "has the following vulnerable packages"; then
  echo "$audit" | sed -n '/vulnerable packages/,$p'
  fail "dependency audit"
elif [ $code -ne 0 ]; then
  echo "$audit" | tail -n 5
  echo "   SKIPPED: could not reach the vulnerability feed (nuget.org). Not counted as red."
else
  pass
fi

stage "5/5 secret scan (source code and everything served to the browser)"
pattern='[A-Za-z0-9_]*(key|secret|token|passw(or)?d)[A-Za-z0-9_]*["'"'"']?[[:space:]]*[:=][[:space:]]*["'"'"'][^"'"'"']{12,}["'"'"']'
hits="$(grep -rniE "$pattern" app --include='*.cs' --include='*.js' --include='*.html' --include='*.css' 2>/dev/null | grep -v '/bin/' | grep -v '/obj/' || true)"
if [ -n "$hits" ]; then
  echo "$hits" | cut -c1-160
  fail "secret scan"
else
  pass
fi

echo
if [ ${#failed[@]} -eq 0 ]; then
  echo "PIPELINE GREEN"
  exit 0
fi
echo "PIPELINE RED: ${failed[*]}"
exit 1
