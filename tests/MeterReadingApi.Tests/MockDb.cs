using MeterReadingApi.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace MeterReadingApi.Tests;

public class MockDb : IDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new(options);
    }
}
