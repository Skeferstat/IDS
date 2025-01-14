using IdsServer.Database;
using Microsoft.EntityFrameworkCore;

namespace IdsServer.Extensions;

/// <summary>
/// Extensions for the migration.
/// </summary>
public static class MigrationExtensions
{
    /// <summary>
    /// Migrate databases.
    /// </summary>
    /// <param name="app">Application.</param>
    /// <returns>Task.</returns>
    public static async Task MigrateDatabasesAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        AppDbContext appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if ((await appDbContext.Database.GetPendingMigrationsAsync()).Any())
        {
            await appDbContext.Database.MigrateAsync();
        }
    }
}