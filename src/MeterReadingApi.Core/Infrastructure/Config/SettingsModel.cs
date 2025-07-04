using System.Diagnostics.CodeAnalysis;
using System.Threading.RateLimiting;

namespace MeterReadingApi.Core.Infrastructure.Config;

public class SettingsModel
{
    public required DatabaseSettingsModel DatabaseSettings { get; set; }

    public required GenericBoundariesModel GenericBoundaries { get; set; }

    public required FixedWindowRateLimiterOptions FixedWindowRateLimit { get; set; }
}