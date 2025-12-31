using BeeGees_WriteNode.Facade.Repository;
using Microsoft.EntityFrameworkCore;

namespace BeeGees_WriteNode
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Initialize("writer_log.txt", LogLevel.All, false);

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

            var entryPoint = new WriterEntryPoint(new Facade.Facade());

            await entryPoint.WaitForWorkAsync();

            // Keep the application running indefinitely (Docker-compatible)
            await Task.Delay(-1);
        }
    }
}
