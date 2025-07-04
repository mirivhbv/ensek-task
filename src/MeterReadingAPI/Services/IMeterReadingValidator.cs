namespace MeterReadingApi.Services;

public interface IMeterReadingValidator
{
    bool IsValid(MeterReadingCsvDto reading);
}
