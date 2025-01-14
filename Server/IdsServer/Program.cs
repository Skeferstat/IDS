using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using IdsServer;
using IdsServer.Extensions;
using IdsServer.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.Configure(logging =>
{
    logging.ActivityTrackingOptions = ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId;
});
builder.Host.UseSerilog(SeriLogger.Configure);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOptions<AppOptions>()
    .BindConfiguration(AppOptions.SectionName)
    .ValidateFluently()
    .ValidateOnStart();


builder.Services.AddMemoryCache(); 

// Add services to the container.
builder.Services
    .AddRazorPages()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();


builder.RegisterServices();
builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

builder.Services.AddMiniProfiler(options => options.RouteBasePath = "/profiler");

builder.RegisterFeatureManagement();

var app = builder.Build();

app.UseProblemDetails();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() == false)
{
    app.UseExceptionHandler("/Error");
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMiniProfiler();
}

await app.MigrateDatabasesAsync();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();