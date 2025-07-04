using System.ComponentModel.DataAnnotations;

namespace MeterReadingApi.Core.Models;

public class MeterReading
{
    [Key]
    public int Id { get; set; }

    public int AccountId { get; set; }
    
    public Account? Account { get; set; }

    public DateTime MeterReadingDateTime { get; set; }

    [RegularExpression(@"^\d{5}$")]
    public required string MeterReadValue { get; set; }
}
