
using MeterReadingApi.Services;
using MeterReadingApi.Core.Infrastructure.Builders;
using MeterReadingApi;
using MeterReadingApi.Core.Infrastructure.Config;
using Microsoft.OpenApi.Models;
using MeterReadingApi.Core.Infrastructure.Pipeline;

var builder = WebApplication.CreateBuilder(args);

GlobalConfiguration.ApiSettings = builder.Configuration.GetSection("ApiSettings").Get<SettingsModel>();
GlobalConfiguration.ApiDocument = builder.Configuration.GetSection("ApiDocument").Get<OpenApiDocument>();

builder.AddLoggingServices();
builder.AddComplianceServices();
builder.Services.AddHealthChecks();
builder.Services.AddDataService();
builder.Services.AddOpenApiServices();
builder.Services.AddRateLimitServices();
builder.Services.AddCorsServices();
builder.Services.AddErrorHandling();

// services
builder.Services.AddScoped<ICsvMeterReadingParser, CsvMeterReadingParser>();
builder.Services.AddScoped<IMeterReadingValidator, MeterReadingValidator>();
builder.Services.AddTransient<MeterReadingService>();

var app = builder.Build();

app.UseMiddleware<ApiVersionHeaderMiddleware>();
app.UseExceptionHandler();
app.UsePathBase(new PathString($"/{GlobalConfiguration.ApiDocument!.Info.Version}"));
app.UseRateLimiter();
app.UseCors();

app.MapOpenApi("/openapi/{documentName}");

app.Services.EnsureDataServiceCreated();

app.MapApi();
app.MapHealthChecks("/health");

app.Run();
