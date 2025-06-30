using HotelGuru.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelGuru.DataContext.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Vendeg> Vendegek { get; set; }
        public DbSet<Recepcios> Recepciosok { get; set; }
        public DbSet<Adminisztrator> Adminisztratorok { get; set; }
        public DbSet<Szoba> Szobak { get; set; }
        public DbSet<Foglalas> Foglalasok { get; set; }
        public DbSet<PluszSzolgaltatas> PluszSzolgaltatasok { get; set; }
        public DbSet<Felhasznalo> Felhasznalok { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1) Discriminator beállítása a Felhasznalo származási hierarchiára
            modelBuilder.Entity<Felhasznalo>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Vendeg>("Vendeg")
                .HasValue<Recepcios>("Recepcios")
                .HasValue<Adminisztrator>("Admin");
            modelBuilder.Entity<Foglalas>()
                .HasOne(f => f.Vendeg)
                .WithMany(v => v.Foglalasok)
                .HasForeignKey(f => f.VendegId);

            modelBuilder.Entity<Foglalas>()
                .HasOne(f => f.Szoba)
                .WithMany(s => s.Foglalasok)
                .HasForeignKey(f => f.SzobaId);

            modelBuilder.Entity<Foglalas>()
                .HasMany(f => f.PluszSzolgaltatasok)
                .WithMany(p => p.Foglalasok)
                .UsingEntity(j => j.ToTable("FoglalasPluszSzolgaltatas"));

            // 3) Egyedi beállítások
            modelBuilder.Entity<Felhasznalo>()
                .HasIndex(f => f.Felhasznalonev)
                .IsUnique();

            modelBuilder.Entity<Vendeg>()
                .HasIndex(v => v.Email)
                .IsUnique();

            modelBuilder.Entity<PluszSzolgaltatas>(entity =>
            {
                entity.Property(e => e.Ar)
                      .HasPrecision(18, 2);
            });

            // Ha van még egyéb konfiguráció, ide jöhet…
        }

    }
}
