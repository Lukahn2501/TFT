using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TFT.DataLoader.Services;
using TFT.Infrastructure.Data;

namespace TFT.DataLoader;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false);
                config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                // Add DbContext
                var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<TftContext>(options =>
                    options.UseNpgsql(connectionString));

                // Add HttpClient
                services.AddHttpClient<CommunityDragonService>();

                // Add services
                services.AddScoped<DataLoaderService>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Information);
            })
            .Build();

        var logger = host.Services.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("TFT Data Loader starting...");

            // Ensure database exists
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TftContext>();
                logger.LogInformation("Ensuring database exists...");
                await context.Database.EnsureCreatedAsync();
                logger.LogInformation("Database ready");
            }

            // Fetch and load data
            using (var scope = host.Services.CreateScope())
            {
                var cdService = scope.ServiceProvider.GetRequiredService<CommunityDragonService>();
                var loaderService = scope.ServiceProvider.GetRequiredService<DataLoaderService>();

                var data = await cdService.FetchTftDataAsync();

                if (data != null)
                {
                    await loaderService.LoadDataAsync(data);
                    logger.LogInformation("✓ Data load completed successfully!");
                }
                else
                {
                    logger.LogError("Failed to fetch data from Community Dragon");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while loading data");
            return;
        }

        logger.LogInformation("Data loader finished");
    }
}
