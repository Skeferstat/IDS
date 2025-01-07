using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IdsSampleClient
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configure host
            var host = CreateHostBuilder().Build();

            // Set up Dependency Injection
            ServiceProvider = host.Services;

            // Run the application
            Application.Run(ServiceProvider.GetRequiredService<MainForm>());
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    IConfigurationRoot config = new ConfigurationBuilder()
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
                        .Build();

                    services.Configure<AppSettings>(config.GetSection("App"));
                    services.AddTransient<MainForm>();
                })
                .UseSerilog(SeriLogger.Configure); 
        }
    }
}