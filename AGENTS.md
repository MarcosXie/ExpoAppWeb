# AGENTS.md

## Scope and source of truth
- This file covers the root solution (`ExpoApp.sln`) and the `ExpoShared` git submodule (`.gitmodules`).
- No existing agent instruction files were found via the requested glob search; use code + CI config as authoritative.

## Architecture map (what is wired where)
- Composition root is `ExpoApp/Program.cs` (ASP.NET Core API + Swagger + JWT + SignalR hubs + middleware).
- `ExpoApp` layers follow `Api -> Application -> Domain -> Repository`, with `ExpoShared` injected alongside for shared features.
- Shared services are registered first (`AddSharedApplication`, `AddSharedInfrastructure`, `AddSharedRepository(false)`), then ExpoApp-specific services (`AddDomain`, `AddApplication`, `AddRepository`).
- `AddSharedRepository(false)` is intentional: ExpoApp registers shared and app AutoMapper profiles in one pipeline (`ExpoApp.Repository/Extensions/DependencyInjection.cs`, `ExpoShared.Repository/Extensions/ProfileInjections.cs`).
- Persistence uses MySQL EF Core contexts: `ExpoSharedContext` base + `UExpoDbContext` (shared tables) + `ExpoAppDbContext` (app tables, ignores shared-specific sets).

## Request/data flow patterns
- Controllers are thin and delegate to services (example: `ExpoApp/Controllers/ExhibitorController.cs`, `ExpoApp/Controllers/UserController.cs`).
- Real-time chat paths use SignalR hubs in `ExpoApp/Hubs/*` and shared chat services in `ExpoShared.Application/Services/Chats/*`.
- Hub message flow commonly: persist via service -> broadcast room event -> send notification updates (`CallCenterChatHub.SendMessage`).
- Push notifications are Firebase-based and sent from hub base class (`BaseGoogleNotificationHub.SendPushNotification`).
- Exceptions are normalized by `ExceptionMiddleware` into `ErrorResponse` with status from `BaseException`.

## Auth and API behavior quirks
- Fallback authorization requires auth globally; public endpoints must explicitly use `[AllowAnonymous]`.
- JWT claim names consumed by app code are literal: `id`, `name`, `email`, `type` (`ExpoShared.Application/Utils/AuthUserHelper.cs`).
- Route casing is mixed (`api/...` and `Api/...`); preserve existing endpoint style in the file you edit.
- Startup seeds a fixed admin account on boot (`SeedDataHelper.BootstrapAdmin`).

## Local dev workflow
- Initialize submodule before builds: `git submodule update --init --recursive`.
- Build solution from repo root: `dotnet build ExpoApp.sln`.
- Run API host: `dotnet run --project ExpoApp/ExpoApp.Api.csproj` (launch settings use `http://localhost:5130`, Swagger at `/swagger`).
- There are currently no test projects discovered (`**/*Test*.csproj` returned none).
- Use `ExpoApp/UExpo.http` only as a minimal smoke-check template; most real endpoints are controller/hub based.

## Deployment and environment integration
- CI/CD is in `.github/workflows/dotnet.yml`: build/publish API artifact, deploy to AWS Elastic Beanstalk, and pack/publish `ExpoApp.Auth.SDK`.
- API runtime integrates AWS (S3/SES/Translate), Azure Speech/Translator keys, and Firebase credentials (`ExpoShared.Infrastructure/Extensions/DependencyInjection.cs`).
- Elastic Beanstalk runtime tuning lives in `.ebextensions/*` and `.platform/nginx/conf.d/proxy.conf` (large upload/body limits and speech native deps).
- CORS is explicitly allowlisted in `Program.cs`; add origins there when enabling new frontends.

## Editing guardrails for this repo
- Keep DI registrations in the corresponding `Extensions/DependencyInjection.cs` file for each project.
- Prefer implementing behavior in application services; controllers/hubs should orchestrate and return transport DTOs.
- If adding mappings, register profiles via `SetProfileBuilder(...)` before `CreateMappers()`.
- Treat `appsettings*.json` as sensitive in practice (contains real-looking connection/JWT values in this repo); avoid copying secrets into logs, docs, or commits.

