using System.Text.RegularExpressions;

namespace MeterReadingAPI.Services;

public partial class MeterReadingValidator : IMeterReadingValidator
{
    private static readonly Regex _regex = CustomRegex();

    public bool IsValid(MeterReadingCsvDto reading)
    {
        return _regex.IsMatch(reading.MeterReadValue);
    }

    [GeneratedRegex("^\\d{5}$", RegexOptions.Compiled)]
    private static partial Regex CustomRegex();
}
