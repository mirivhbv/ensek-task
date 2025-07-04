using MeterReadingApi.Core.Data;
using MeterReadingApi.Core.Models;
using MeterReadingAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MeterReadingApi.Services;

public class MeterReadingService(AppDbContext context,
    ICsvMeterReadingParser parser,
    IMeterReadingValidator validator)
{
    private readonly AppDbContext _context = context;
    private readonly ICsvMeterReadingParser _parser = parser;
    private readonly IMeterReadingValidator _validator = validator;

    public async Task<MeterReadingUploadResult> ProcessCsvAsync(IFormFile file)
    {
        int success = 0, failed = 0;
        using var stream = file.OpenReadStream();
        var records = await _parser.ParseAsync(stream);

        foreach (var record in records)
        {
            try
            {
                if (!_validator.IsValid(record))
                {
                    failed++;
                    continue;
                }

                var account = await _context.Accounts.FindAsync(record.AccountId);
                if (account == null)
                {
                    failed++;
                    continue;
                }

                bool duplicate = await _context.MeterReadings.AnyAsync(m =>
                    m.AccountId == record.AccountId &&
                    m.MeterReadingDateTime == record.MeterReadingDateTime &&
                    m.MeterReadValue == record.MeterReadValue);

                if (duplicate)
                {
                    failed++;
                    continue;
                }

                var latest = await _context.MeterReadings
                    .Where(m => m.AccountId == record.AccountId)
                    .OrderByDescending(m => m.MeterReadingDateTime)
                    .FirstOrDefaultAsync();

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

                _context.MeterReadings.Add(reading);
                success++;
            }
            catch
            {
                failed++;
            }
        }

        await _context.SaveChangesAsync();

        return new MeterReadingUploadResult
        {
            SuccessfulReadings = success,
            FailedReadings = failed
        };
    }
}
