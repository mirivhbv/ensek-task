using MeterReadingApi.Core.Data;
using MeterReadingApi.Core.Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace MeterReadingApi.Core.Infrastructure.Builders;

public static partial class DataBuilder
{
    public static IServiceCollection AddDataService(this IServiceCollection services)
    {
        switch (GlobalConfiguration.ApiSettings!.DatabaseSettings.DbType)
        {
            case "Npgsql":
                {
                    services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(GlobalConfiguration.ApiSettings!.DatabaseSettings.ConnectionString));
                    break;
                }
            case "SQLite":
                {
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase("MeterReadingApi"));
                    break;
                }

            default:
                throw new NotSupportedException($"Database type '{GlobalConfiguration.ApiSettings.DatabaseSettings.DbType}' is not supported.");
        }

        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }

    public static void EnsureDataServiceCreated(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
    }
}
