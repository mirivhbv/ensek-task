namespace MeterReadingApi.DTOs;

public class MeterReadingUploadResult
{
    public int SuccessfulReadings { get; set; }
    
    public int FailedReadings { get; set; }
}
