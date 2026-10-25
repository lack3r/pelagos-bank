# Pelagos Bank — security lab

**Ground rules (read first).** Attack only this lab, running on your own machine.
Never touch your employer's network, or any other real network. Localhost only.

Pelagos Bank is a small, deliberately vulnerable API for a fictional bank. It exists
so you can practise finding and fixing real security flaws in a .NET service. Nothing
here is a real bank, and nothing here should ever be pointed at a real system.

## Prerequisites

- .NET 10 SDK
- First run only, for the HTTPS endpoint: `dotnet dev-certs https --trust`

## Run it

```bash
dotnet run --project app/Pelagos.Api
```

That serves the API on http://localhost:15080. To also expose the HTTPS endpoint on
https://localhost:15443, start the `https` profile instead:

```bash
dotnet run --project app/Pelagos.Api --launch-profile https
```

Either way it runs in the Production environment, exactly as it would in the wild.

## Then

Open **HUNT.md** for how the exercise works and the scorecard to fill in.
