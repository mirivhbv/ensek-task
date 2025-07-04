namespace MeterReadingApi.Core.Infrastructure.Config;

public class GenericBoundariesModel
{
    public int Minimum { get; set; }
    public int Maximum { get; set; }
    public string Regex { get; set; } = "";
}