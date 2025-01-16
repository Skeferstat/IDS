using Microsoft.Extensions.Hosting;
using Serilog;

namespace IdsSampleClient.Logging;

/// <summary>
/// SeriLogger configuration.
/// </summary>
public static class SeriLogger
{
    /// <summary>
    /// Configure SeriLogger.
    /// </summary>
    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
        (context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        };
}
