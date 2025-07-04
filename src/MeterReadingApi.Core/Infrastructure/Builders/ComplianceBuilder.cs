using System;

namespace MeterReadingApi.Core.Infrastructure.Builders;

public static partial class ComplianceBuilder
{
    public static IHostApplicationBuilder AddComplianceServices(this IHostApplicationBuilder builder)
    {
        // Enable redaction of `[LogProperties]` objects
        builder.Logging.EnableRedaction();

        // Add the redaction services
        builder.Services.AddRedaction(builder =>
        {
            // todo
            // add critical meter values to redact
        });

        return builder;
    }
}
