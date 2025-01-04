using AutoMapper;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using IdsServer.Mapping;
using Microsoft.FeatureManagement;

namespace IdsServer.Extensions;

/// <summary>
/// Api extensions.
/// </summary>
public static class ApiExtensions
{
    /// <summary>
    /// Register services.
    /// </summary>
    /// <param name="builder">Web application builder.</param>
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
      
        builder.Services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });


        builder.Services.AddControllers(options =>
        {

        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
        });

        builder.Services.AddHealthChecks();
        builder.Services.AddMemoryCache();
        builder.Services.AddHttpClient();
        builder.Services.AddProblemDetails(options =>
        {
            options.IncludeExceptionDetails = (context, ex) =>
            {
                var env = context.RequestServices.GetRequiredService<IHostEnvironment>();
                return env.IsDevelopment() || env.IsStaging();
            };
            // This will map NotImplementedException to the 501 Not Implemented status code.
            options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
            // This will map HttpRequestException to the 503 Service Unavailable status code.
            options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
            // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
            // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);

        });
        builder.Services.AddProblemDetailsConventions();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
 

        builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);
        builder.Services.AddHttpContextAccessor();

        builder.Services.RegisterMapper();
    }

    /// <summary>
    /// Register AutoMapper profiles.
    /// </summary>
    /// <param name="services">Services collection.</param>
    /// <returns>Services collection.</returns>
    public static IServiceCollection RegisterMapper(this IServiceCollection services)
    {
        MapperConfiguration mapperConfig = new(mc =>
        {
            mc.AddProfile<BasketReceiveMappingProfile>();
        });
        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
        return services;
    }

    

    public static void RegisterFeatureManagement(this WebApplicationBuilder builder)
    {
        builder.Services.AddFeatureManagement(builder.Configuration.GetSection("FeatureFlags"));
    }
}