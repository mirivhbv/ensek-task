using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace MeterReadingApi.Services;

public class CsvMeterReadingParser : ICsvMeterReadingParser
{
    public async Task<IEnumerable<MeterReadingCsvDto>> ParseAsync(Stream csvStream)
    {
        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            IgnoreBlankLines = true
        });
        var records = new List<MeterReadingCsvDto>();
        await foreach (var record in csv.GetRecordsAsync<MeterReadingCsvDto>())
        {
            records.Add(record);
        }
        return records;
    }
}
