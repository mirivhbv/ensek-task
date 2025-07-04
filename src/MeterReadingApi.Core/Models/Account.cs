using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MeterReadingApi.Core.Models;

public class Account
{
    [Key]
    public int AccountId { get; set; }

    [Description("The first name of the account holder.")]
    [MaxLength(60)]
    public required string FirstName { get; set; }
    
    [Description("The last name of the account holder.")]
    [MaxLength(60)]
    public required string LastName { get; set; }

    public ICollection<MeterReading> MeterReadings { get; set; } = [];
}
