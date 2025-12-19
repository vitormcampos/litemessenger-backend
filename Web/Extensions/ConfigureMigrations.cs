using Application;
using Microsoft.EntityFrameworkCore;

namespace Web.Extensions;

public static class ConfigureMigrations
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FinanceiroContext>();

        // Check and apply pending migrations
        var pendingMigrations = context.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            Console.WriteLine("Applying pending migrations...");
            context.Database.Migrate();
            Console.WriteLine("Migrations applied successfully.");
        }
        else
        {
            Console.WriteLine("No pending migrations found.");
        }

        return app;
    }
}
