namespace MeterReadingAPI.Services;

public interface ICsvMeterReadingParser
{
    Task<IEnumerable<MeterReadingCsvDto>> ParseAsync(Stream csvStream);
}
