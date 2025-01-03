using Serilog;

namespace IdsServer.Logging;

/// <summary>
/// SeriLogger.
/// </summary>
public static class SeriLogger
{
    /// <summary>
    /// Configure SeriLogger.
    /// </summary>
    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
        (context, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration);
        };
}