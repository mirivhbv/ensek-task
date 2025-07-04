using Microsoft.OpenApi.Models;

namespace MeterReadingApi.Core.Infrastructure.Config;

public static class GlobalConfiguration
{
    public static object? ApiExamples { get; set; }
    
    public static OpenApiDocument? ApiDocument { get; set; }

    public static SettingsModel? ApiSettings { get; set; }
}