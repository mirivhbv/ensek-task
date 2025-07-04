using MeterReadingApi.Core.Infrastructure.Config;
using MeterReadingApi.Core.Infrastructure.OpenApi.Transformers;
using Scalar.AspNetCore;

namespace MeterReadingApi.Core.Infrastructure.Builders;

public static partial class OpenApiBuilder
{
    public static IServiceCollection AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApi(GlobalConfiguration.ApiDocument!.Info.Version, options =>
        {
            options.AddDocumentTransformer<TransformerDocInfo>();
            options.AddSchemaTransformer<TransformerExampleSchema>();
            options.AddOperationTransformer<TransformerOperation>();
        });

        return services;
    }

    public static void AddOpenApiScalarReference(this IEndpointRouteBuilder app)
    {
        app.MapScalarApiReference(options =>
        {
            options.Theme = ScalarTheme.DeepSpace;
            options.AddApiKeyAuthentication("ApiToken", key => key.Value = "Lifetime Subscription");
            options.Title = $"{GlobalConfiguration.ApiDocument!.Info.Title} docs | {GlobalConfiguration.ApiDocument.Info.Version}";
        });
    }
}