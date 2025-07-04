using System.Text;
using MeterReadingApi.Services;

namespace MeterReadingApi.Tests;

public class CsvMeterReadingParserTests
{
    [Fact]
    public async Task ParseAsync_ParsesValidCsv()
    {
        var csv = "AccountId,MeterReadingDateTime,MeterReadValue\n1,2024-01-01 00:00,12345\n2,2024-01-02 00:00,54321";
        var bytes = Encoding.UTF8.GetBytes(csv);
        using var stream = new MemoryStream(bytes);
        var parser = new CsvMeterReadingParser();
        var result = (await parser.ParseAsync(stream)).ToList();
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].AccountId);
        Assert.Equal("12345", result[0].MeterReadValue);
        Assert.Equal(2, result[1].AccountId);
        Assert.Equal("54321", result[1].MeterReadValue);
    }

    [Fact]
    public async Task ParseAsync_EmptyCsv_ReturnsEmpty()
    {
        var csv = "AccountId,MeterReadingDateTime,MeterReadValue\n";
        var bytes = Encoding.UTF8.GetBytes(csv);
        using var stream = new MemoryStream(bytes);
        var parser = new CsvMeterReadingParser();
        var result = (await parser.ParseAsync(stream)).ToList();
        Assert.Empty(result);
    }
}
