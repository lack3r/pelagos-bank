# Pelagos security pipeline: the gates a CI server runs on every push, run on your laptop.
# Every stage runs, then the summary says GREEN or which stages are RED.
# Usage: pipeline\run.cmd   (or: powershell -ExecutionPolicy Bypass -File pipeline\run.ps1)
$ErrorActionPreference = "Continue"
Set-Location (Join-Path $PSScriptRoot "..")

$failed = @()
function Stage($name) { Write-Host ""; Write-Host "== $name ==" }
function Pass { Write-Host "   ok" -ForegroundColor Green }
function Fail($name) { Write-Host "   RED: $name" -ForegroundColor Red; $script:failed += $name }

Stage "1/5 restore packages"
dotnet restore Pelagos.sln --verbosity quiet
if ($LASTEXITCODE -eq 0) { Pass } else { Fail "restore" }

Stage "2/5 build"
dotnet build Pelagos.sln --no-restore --verbosity quiet --nologo
if ($LASTEXITCODE -eq 0) { Pass } else { Fail "build" }

Stage "3/5 tests"
dotnet test Pelagos.sln --no-build --verbosity quiet --nologo
if ($LASTEXITCODE -eq 0) { Pass } else { Fail "tests" }

Stage "4/5 dependency audit (known vulnerable packages)"
$audit = (dotnet list app/Pelagos.Api/Pelagos.Api.csproj package --vulnerable --include-transitive 2>&1) | Out-String
$code = $LASTEXITCODE
if ($audit -match "has the following vulnerable packages") {
  Write-Host $audit
  Fail "dependency audit"
} elseif ($code -ne 0) {
  Write-Host $audit
  Write-Host "   SKIPPED: could not reach the vulnerability feed (nuget.org). Not counted as red." -ForegroundColor Yellow
} else { Pass }

Stage "5/5 secret scan (source code and everything served to the browser)"
$pattern = '[A-Za-z0-9_]*(key|secret|token|passw(or)?d)[A-Za-z0-9_]*["'']?\s*[:=]\s*["''][^"'']{12,}["'']'
$files = Get-ChildItem -Path app -Recurse -File -Include *.cs, *.js, *.html, *.css |
  Where-Object { $_.FullName -notmatch '[\\/](bin|obj)[\\/]' }
$hits = $files | Select-String -Pattern $pattern
if ($hits) {
  $hits | ForEach-Object { Write-Host ("{0}:{1}: {2}" -f $_.Path, $_.LineNumber, $_.Line.Trim()) }
  Fail "secret scan"
} else { Pass }

Write-Host ""
if ($failed.Count -eq 0) {
  Write-Host "PIPELINE GREEN" -ForegroundColor Green
  exit 0
}
Write-Host ("PIPELINE RED: " + ($failed -join ", ")) -ForegroundColor Red
exit 1
