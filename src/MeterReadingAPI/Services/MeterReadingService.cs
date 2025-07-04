using MeterReadingApi.Core.Data;
using MeterReadingApi.Core.Models;
using MeterReadingApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MeterReadingApi.Services;

public class MeterReadingService(AppDbContext context,
    ICsvMeterReadingParser parser,
    IMeterReadingValidator validator)
{

    public async Task<MeterReadingUploadResult> ProcessCsvAsync(IFormFile file)
    {
        int success = 0, failed = 0;
        using var stream = file.OpenReadStream();
        var records = await parser.ParseAsync(stream);

        // Track readings added in this batch
        var batchReadings = new List<MeterReading>();

        foreach (var record in records)
        {
            try
            {
                if (!validator.IsValid(record))
                {
                    failed++;
                    continue;
                }

                var account = await context.Accounts.FindAsync(record.AccountId);
                if (account == null)
                {
                    failed++;
                    continue;
                }

                // Check for duplicate in DB or in this batch
                bool duplicate = await context.MeterReadings.AnyAsync(m =>
                    m.AccountId == record.AccountId &&
                    m.MeterReadingDateTime == record.MeterReadingDateTime &&
                    m.MeterReadValue == record.MeterReadValue)
                    || batchReadings.Any(m =>
                        m.AccountId == record.AccountId &&
                        m.MeterReadingDateTime == record.MeterReadingDateTime &&
                        m.MeterReadValue == record.MeterReadValue);

                if (duplicate)
                {
                    failed++;
                    continue;
                }

                // Check for out-of-order in DB or in this batch
                var latestDb = await context.MeterReadings
                    .Where(m => m.AccountId == record.AccountId)
                    .OrderByDescending(m => m.MeterReadingDateTime)
                    .FirstOrDefaultAsync();
                var latestBatch = batchReadings
                    .Where(m => m.AccountId == record.AccountId)
                    .OrderByDescending(m => m.MeterReadingDateTime)
                    .FirstOrDefault();
                var latest = latestDb;
                if (latestBatch != null && (latestDb == null || latestBatch.MeterReadingDateTime > latestDb.MeterReadingDateTime))
                    latest = latestBatch;

                if (latest != null && record.MeterReadingDateTime < latest.MeterReadingDateTime)
                {
                    failed++;
                    continue;
                }

                var reading = new MeterReading
                {
                    AccountId = record.AccountId,
                    MeterReadingDateTime = record.MeterReadingDateTime,
                    MeterReadValue = record.MeterReadValue
                };

                batchReadings.Add(reading);
                context.MeterReadings.Add(reading);
                success++;
            }
            catch
            {
                failed++;
            }
        }

        await context.SaveChangesAsync();

        return new MeterReadingUploadResult
        {
            SuccessfulReadings = success,
            FailedReadings = failed
        };
    }
}
