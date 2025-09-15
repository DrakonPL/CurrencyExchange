using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CurrencyExchange.Infrastructure
{
    // Ensures design-time creation of DbContext works for migrations (dotnet ef) without needing the full host.
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CurrencyExchangeDbContext>
    {
        public CurrencyExchangeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CurrencyExchangeDbContext>();
            var dbPath = System.IO.Path.Join(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CurrencyExchange.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
            return new CurrencyExchangeDbContext(optionsBuilder.Options);
        }
    }
}
