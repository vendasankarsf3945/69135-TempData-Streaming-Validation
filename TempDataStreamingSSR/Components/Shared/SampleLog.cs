using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class SampleLog
{
    public static string ResolveStorageMode(HttpContext? httpContext, string fallback = "Cookie")
    {
        var configuredMode = httpContext?.RequestServices
            .GetService<IConfiguration>()
            ?["TempDataProvider"];

        if (string.Equals(configuredMode, "Session", StringComparison.OrdinalIgnoreCase))
        {
            return "Session";
        }

        if (string.Equals(configuredMode, "Cookie", StringComparison.OrdinalIgnoreCase))
        {
            return "Cookie";
        }

        return fallback;
    }

    public static void Info(
        ILogger logger,
        string scenario,
        string storageMode,
        string checkpoint,
        object? value,
        HttpContext? httpContext)
    {
        logger.LogInformation(
            "Timestamp={Timestamp:O}; Scenario={Scenario}; StorageMode={StorageMode}; Checkpoint={Checkpoint}; Response.HasStarted={HasStarted}; Value={Value}",
            DateTimeOffset.UtcNow,
            scenario,
            storageMode,
            checkpoint,
            httpContext?.Response.HasStarted,
            value);
    }
}