using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public static class SampleLog
{
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