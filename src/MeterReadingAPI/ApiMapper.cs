using MeterReadingApi.Services;

namespace MeterReadingApi;

public static partial class ApiMapper
{
    public static WebApplication MapApi(this WebApplication app)
    {
        app.MapPost("/meter-reading-uploads", async (IFormFile file, MeterReadingService service) =>
        {
            if (file.Length == 0)
            {
                return Results.BadRequest("File is empty.");
            }

            try
            {
                var result = await service.ProcessCsvAsync(file);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        return app;
    }
}
