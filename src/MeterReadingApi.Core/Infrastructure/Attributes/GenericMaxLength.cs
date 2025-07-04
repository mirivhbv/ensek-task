using System.ComponentModel.DataAnnotations;
using MeterReadingApi.Core.Infrastructure.Config;

namespace MeterReadingApi.Core.Infrastructure.Attributes;

public class GenericMaxLengthAttribute : MaxLengthAttribute
{
    public GenericMaxLengthAttribute()
        : base(GlobalConfiguration.ApiSettings!.GenericBoundaries.Maximum)
        {
        }
}