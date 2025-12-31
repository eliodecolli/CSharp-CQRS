using BeeGees_ReadNode.Facade.Repository;
using Microsoft.EntityFrameworkCore;

namespace BeeGees_ReadNode
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Initialize("log_reader.txt", LogLevel.All, false);

            // Apply database migrations
            Log.Info("Applying database migrations...");
            using (var context = new ShipmentContext())
            {
                await context.Database.MigrateAsync();
            }
            Log.Info("Database migrations applied successfully.");

            // Warm up database connection to avoid cold start delays
            Log.Info("Warming up database connection...");
            using (var context = new ShipmentContext())
            {
                // Execute a simple query to initialize EF and database connection pool
                _ = await context.Shipments.AnyAsync();
            }
            Log.Info("Database connection warmed up successfully.");

            var entryPoint = new ReaderEntryPoint();
            await entryPoint.WaitForConnectionsAsync();

            // Keep the application running indefinitely (Docker-compatible)
            await Task.Delay(-1);
        }
    }
}
