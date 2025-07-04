using MeterReadingApi.Core.Infrastructure.Config;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace MeterReadingApi.Core.Infrastructure.OpenApi.Transformers;

class TransformerExampleSchema : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (GlobalConfiguration.ApiExamples is not OpenApiObject apiExamples)
            return Task.CompletedTask;

        // todo: implement it

        return Task.CompletedTask;
    }
}
