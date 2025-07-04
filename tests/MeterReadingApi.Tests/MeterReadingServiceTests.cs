using System.Text;
using MeterReadingApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using MeterReadingApi.Core.Data;
using MeterReadingApi.Core.Models;

namespace MeterReadingApi.Tests;

public class MeterReadingServiceTests : IDisposable
{
    private readonly AppDbContext _db;

    public MeterReadingServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new AppDbContext(options);
        _db.Accounts.Add(new Account { AccountId = 1, FirstName = "Test", LastName = "User" });
        _db.SaveChanges();
    }

    public void Dispose()
    {
        _db.Database.EnsureDeleted();
        _db.Dispose();
    }

    private static IFormFile CreateCsvFile(string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);
        return new FormFile(stream, 0, bytes.Length, "file", "test.csv");
    }

    [Fact]
    public async Task ProcessCsvAsync_ValidReading_Success()
    {
        var parser = new CsvMeterReadingParser();
        var validator = new MeterReadingValidator();
        var service = new MeterReadingService(_db, parser, validator);
        var csv = "AccountId,MeterReadingDateTime,MeterReadValue\n1,2024-01-01 00:00,12345";
        var file = CreateCsvFile(csv);
        var result = await service.ProcessCsvAsync(file);
        Assert.Equal(1, result.SuccessfulReadings);
        Assert.Equal(0, result.FailedReadings);
    }

    [Fact]
    public async Task ProcessCsvAsync_InvalidAccount_Fails()
    {
        var parser = new CsvMeterReadingParser();
        var validator = new MeterReadingValidator();
        var service = new MeterReadingService(_db, parser, validator);
        var csv = "AccountId,MeterReadingDateTime,MeterReadValue\n999,2024-01-01 00:00,12345";
        var file = CreateCsvFile(csv);
        var result = await service.ProcessCsvAsync(file);
        Assert.Equal(0, result.SuccessfulReadings);
        Assert.Equal(1, result.FailedReadings);
    }

    [Fact]
    public async Task ProcessCsvAsync_DuplicateReading_Fails()
    {
        var parser = new CsvMeterReadingParser();
        var validator = new MeterReadingValidator();
        var service = new MeterReadingService(_db, parser, validator);
        var csv = "AccountId,MeterReadingDateTime,MeterReadValue\n1,2024-01-01 00:00,12345\n1,2024-01-01 00:00,12345";
        var file = CreateCsvFile(csv);
        var result = await service.ProcessCsvAsync(file);
        Assert.Equal(1, result.SuccessfulReadings);
        Assert.Equal(1, result.FailedReadings);
    }

    [Fact]
    public async Task ProcessCsvAsync_OutOfOrderReading_Fails()
    {
        var parser = new CsvMeterReadingParser();
        var validator = new MeterReadingValidator();
        var service = new MeterReadingService(_db, parser, validator);
        var csv = "AccountId,MeterReadingDateTime,MeterReadValue\n1,2024-01-02 00:00,12345\n1,2024-01-01 00:00,12346";
        var file = CreateCsvFile(csv);
        var result = await service.ProcessCsvAsync(file);
        Assert.Equal(1, result.SuccessfulReadings);
        Assert.Equal(1, result.FailedReadings);
    }

    [Fact]
    public async Task ProcessCsvAsync_InvalidMeterValue_Fails()
    {
        var parser = new CsvMeterReadingParser();
        var validator = new MeterReadingValidator();
        var service = new MeterReadingService(_db, parser, validator);
        var csv = "AccountId,MeterReadingDateTime,MeterReadValue\n1,2024-01-01 00:00,abcde";
        var file = CreateCsvFile(csv);
        var result = await service.ProcessCsvAsync(file);
        Assert.Equal(0, result.SuccessfulReadings);
        Assert.Equal(1, result.FailedReadings);
    }
}
