using MeterReadingApi.Services;

namespace MeterReadingApi.Tests;

public class MeterReadingValidatorTests
{
    private readonly MeterReadingValidator _validator = new();

    [Theory]
    [InlineData("12345", true)]
    [InlineData("00000", true)]
    [InlineData("99999", true)]
    [InlineData("1234", false)]
    [InlineData("123456", false)]
    [InlineData("12a45", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValid_ReturnsExpectedResult(string value, bool expected)
    {
        var dto = new MeterReadingCsvDto { MeterReadValue = value ?? string.Empty };
        var result = _validator.IsValid(dto);
        Assert.Equal(expected, result);
    }
}
