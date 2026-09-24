# TempData Streaming Validation Sample

This repository contains a Blazor Web App sample used to validate how TempData and session-backed state behave under streaming SSR.

The sample focuses on these scenarios:

- Cookie-backed TempData written before and after a delayed flush
- Session-backed positive and negative flows
- Nonstreaming comparisons for the same value paths
- A late-mounted child component that attempts a session write after the response has started

## Current Behavior

- `Program.cs` switches TempData behavior using the `TempDataProvider` configuration value.
- Session services are always registered and `UseSession()` is always enabled.
- `appsettings.json` currently sets `TempDataProvider` to `Cookie`; the `http-session` and `https-session` launch profiles override it to `Session`.
- The late-session negative case mounts the child after streaming has started, performs a deferred assignment, and then reads the default value back on a follow-up page.

## Prerequisites

- .NET SDK `11.0.100-rc.1.26425.128`
- A browser with developer tools for inspecting cookies and network timing

## Build

From `TempDataStreamingSSR`:

```powershell
dotnet build TempDataStreamingSSR.csproj
```

## Run

The repo currently includes these launch profiles:

- `http`
- `https`
- `http-session`
- `https-session`

Run a profile from `TempDataStreamingSSR` with:

```powershell
dotnet run --launch-profile http-session
```

To force cookie-backed TempData, set `TempDataProvider=Cookie` in your environment or local appsettings override before launching.

## Routes

- `/tempdata-streaming-write`
- `/tempdata-reader`
- `/tempdata-early-streaming`
- `/tempdata-early-nonstreaming`
- `/session-primer`
- `/session-streaming-positive`
- `/session-late-parent`
- `/session-late-reader`
- `/session-nonstreaming`

## Logging Pattern

The sample uses a shared helper to make ordering easier to read in console output. Important events are logged with:

- `Timestamp`
- `Scenario`
- `StorageMode`
- `Checkpoint`
- `Response.HasStarted`
- `Value`

Use those logs together with browser screenshots or network traces to verify ordering instead of relying on a single label like `Response Started`. `StorageMode` is resolved from the active `TempDataProvider` configuration on each request, so session-mode runs log `Session` and cookie-mode runs log `Cookie`.

## Validation Order

1. Start from a fresh browser profile or cleared site data.
2. Capture the initial request and confirm no prior session cookie exists.
3. Capture the response that establishes the cookie or session.
4. Capture the timestamped checkpoints for the delayed write.
5. For the negative late-session case, verify the first late-session render starts from a clean session and that the follow-up reader still shows the default value after the late assignment occurs.

## Evidence Layout

- `Evidence/Environment`: SDK and environment information
- `Evidence/Screenshots/cookie_post_await_variant`: cookie-backed delayed write evidence
- `Evidence/Screenshots/cookie_pre_await_variant`: cookie-backed early assignment evidence
- `Evidence/Screenshots/session_early_vs_deferred_child`: session ordering and deferred-child evidence
- `Evidence/Build-logs`: [Sample build information](https://github.com/vendasankarsf3945/69135-TempData-Streaming-Validation/tree/main/Evidence/Build-logs/build-log.png)
