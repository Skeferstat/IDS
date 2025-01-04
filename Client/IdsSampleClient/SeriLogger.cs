using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdsSampleClient;
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
