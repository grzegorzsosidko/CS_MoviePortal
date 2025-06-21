using Firma.Data.Data.AppSettings;
using Firma.Data.Data.Customers;
using Firma.Data.Data.Movie;
using Firma.Data.Data.Ulubione;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Data.Data
{
    public class FirmaContext : DbContext
    {
        public FirmaContext(DbContextOptions<FirmaContext> options) 
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = default!;
        public DbSet<Film> Film { get; set; } = default!;
        public DbSet<Gatunek> Gatunek { get; set; } = default!;
        public DbSet<Aktor> Aktor { get; set; } = default!;
        public DbSet<Reżyser> Reżyser { get; set; } = default!;
        public DbSet<Recenzja> Recenzja { get; set; } = default!;
        public DbSet<CMS.Strona> Strona { get; set; } = default!;
        public DbSet<Ustawienie> Ustawienie { get; set; } = default!;
        public DbSet<UlubionyFilm> UlubioneFilmy { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Wstępne dane (seeding) dla ustawień
            modelBuilder.Entity<Ustawienie>().HasData(
                new Ustawienie { IdUstawienia = 1, Klucz = "WelcomeHeadline", Wartosc = "Witaj w MoviePortal!" },
                new Ustawienie { IdUstawienia = 2, Klucz = "WelcomeText", Wartosc = "Twoja baza najlepszych filmów." },
                new Ustawienie { IdUstawienia = 3, Klucz = "BannerText", Wartosc = "Odkryj świat kina z nami. Przeglądaj, oceniaj i dodawaj recenzje swoich ulubionych filmów." }
            );

            modelBuilder.Entity<UlubionyFilm>()
       .HasIndex(uf => new { uf.IdUzytkownika, uf.IdFilmu })
       .IsUnique();
        }
    }


}
