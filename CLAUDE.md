# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

`enigma` is a two-project solution generated from the Visual Studio "Angular and ASP.NET Core" template:

- **enigma.Server** — ASP.NET Core 10 Web API (`enigma.Server/enigma.Server.csproj`). Serves the API and, in production, the built Angular static files.
- **enigma.client** — Angular 19 SPA (`enigma.client/`), non-standalone components/directives/pipes (schematics default to `standalone: false`).

The two are wired together via **SpaProxy**: `enigma.Server` references `Microsoft.AspNetCore.SpaProxy` and sets `SpaProxyServerUrl` to `https://localhost:53746` (must match the Angular dev server's `serve.options.port` in `enigma.client/angular.json`). When you run the server in Development, it proxies non-API requests to the Angular CLI dev server, which it launches via `SpaProxyLaunchCommand` (`npm start`). If these three pieces (csproj `SpaRoot`/`SpaProxyServerUrl`, `launchSettings.json`'s `ASPNETCORE_HOSTINGSTARTUPASSEMBLIES`, and the Angular dev server port) drift out of sync, the SPA proxy fails at startup — see the "Fix SpaProxy startup failure" commit for the shape of that failure.

The client's dev-server proxy config (`enigma.client/src/proxy.conf.js`) forwards specific paths (currently `/weatherforecast`) to the ASP.NET Core backend, using `ASPNETCORE_HTTPS_PORT`/`ASPNETCORE_URLS` env vars to find the backend's port. When adding new API routes/controllers, add their path prefix to this proxy config too, or client dev-server requests to them will 404.

Solution file is `enigma.slnx` (new XML-based slnx format, not `.sln`).

## Commands

Run these from the respective project directory.

### enigma.Server (.NET)

```bash
dotnet build                # build
dotnet run                  # run (Development profile per launchSettings.json)
dotnet watch run            # run with hot reload
```

No server-side test project exists yet.

### enigma.client (Angular)

```bash
npm install                 # install deps
npm start                   # ng serve with HTTPS dev certs (platform-specific script, see below)
ng build                    # production build -> dist/enigma.client
ng build --configuration development
ng test                     # Karma/Jasmine unit tests, single run against Chrome
ng test --no-watch --code-coverage   # CI-style single run with coverage
```

- `npm start` runs `prestart` (`node aspnetcore-https` — generates/imports the ASP.NET Core dev HTTPS cert) then a platform-specific `start:windows`/`start:default` script via `run-script-os`, launching `ng serve` bound to `127.0.0.1` with that cert.
- To run a single spec file with Karma, there's no built-in CLI flag for filtering by file; use Jasmine's `fdescribe`/`fit` in the spec to focus a run, or pass `--include=path/to/file.spec.ts` to `ng test`.
- No e2e framework is configured.

### Running the full app together

Launch via the solution (Visual Studio "https" launch profile, or `dotnet run` from `enigma.Server` with `ASPNETCORE_HOSTINGSTARTUPASSEMBLIES=Microsoft.AspNetCore.SpaProxy` as set in `launchSettings.json`) — this starts the API and auto-launches the Angular dev server through SpaProxy. Don't run `ng serve` and `dotnet run` manually as two independent processes unless you're intentionally bypassing the proxy.

## Architecture notes

- `enigma.Server/Program.cs` is minimal hosting setup: controllers, OpenAPI (`/openapi/{documentName}.json` in Development only), HTTPS redirection, static file serving (`UseDefaultFiles`/`MapStaticAssets`), and `MapFallbackToFile("/index.html")` for SPA client-side routing fallback.
- Controllers live under `enigma.Server/Controllers/`, one file per controller, using the standard `[ApiController][Route("[controller]")]` pattern (see `WeatherForecastController.cs`, currently the only endpoint).
- Angular app is still the CLI-generated skeleton — `AppModule`/`AppComponent`/`AppRoutingModule` under `enigma.client/src/app/`, no feature modules yet.
- Client build output goes to `enigma.client/dist/enigma.client/browser/` (set via `BuildOutputFolder` in `enigma.client.esproj`); the server's static-file serving expects the published SPA there in non-dev environments.

## Azure

Per `.github/copilot-instructions.md`: for any Azure-related task, use Azure MCP tools, and call the best-practices tool first if available.

## Backend Architecture
- Repository pattern + Service layer (no direct DbContext access from controllers)
- EF Core, code-first (migrations, not database-first)
- Unit tests mandatory for the service layer (xUnit + Moq or NSubstitute)

## Database
- Azure SQL Server
- **IMPORTANT CONSTRAINT: minimal budget.** Use low-cost tiers (Serverless with auto-pause, or Basic/S0). Do NOT propose Premium, Business Critical, or additional Azure services (Redis, extended Application Insights, etc.) without asking explicitly first.
- Connection strings in `appsettings.Development.json` locally / User Secrets, NOT hardcoded.

## Authentication & Authorization
- ASP.NET Core Identity for user management
- JWT for API authentication (Client is an SPA, not server-rendered)
- Roles: role management via Identity (RoleManager)
- Endpoints: register, login, refresh token, role assignment
