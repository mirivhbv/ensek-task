namespace MeterReadingAPI.Services;

public interface IMeterReadingValidator
{
    bool IsValid(MeterReadingCsvDto reading);
}
