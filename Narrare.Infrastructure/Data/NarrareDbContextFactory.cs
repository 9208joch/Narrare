using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Narrare.Infrastructure.Data;

public class NarrareDbContextFactory
    : IDesignTimeDbContextFactory<NarrareDbContext>
{
    public NarrareDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<NarrareDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Database=NarrareDb;Trusted_Connection=True;TrustServerCertificate=True");

        return new NarrareDbContext(optionsBuilder.Options);
    }
}