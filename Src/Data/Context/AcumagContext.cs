using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class AcumagContext : DbContext
{
    public AcumagContext(DbContextOptions<AcumagContext> options) : base(options)
    {
    }
}