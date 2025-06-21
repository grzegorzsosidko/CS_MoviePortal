using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Firma.Data.Data
{
    // Ta klasa służy tylko do tego, aby narzędzia deweloperskie (jak Add-Migration)
    // wiedziały, jak utworzyć instancję DbContextu.
    public class FirmaContextFactory : IDesignTimeDbContextFactory<FirmaContext>
    {
        public FirmaContext CreateDbContext(string[] args)
        {
            // Tworzy obiekt opcji, który będzie konfigurował DbContext
            var optionsBuilder = new DbContextOptionsBuilder<FirmaContext>();

            // Na sztywno ustawia connection string, którego mają używać narzędzia.
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MoviePortalDB;Trusted_Connection=True;MultipleActiveResultSets=true");

            // Zwracam nową instancję DbContextu z tak przygotowanymi opcjami.
            return new FirmaContext(optionsBuilder.Options);
        }
    }
}