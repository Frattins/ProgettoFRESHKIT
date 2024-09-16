using JerseyShop.Models; 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JerseyShop.Data
{
    public class FootballShopContext : IdentityDbContext
    {
        public FootballShopContext(DbContextOptions<FootballShopContext> options) : base(options) { }

        public DbSet<Maglia> Maglie { get; set; }
        public DbSet<Ordine> Ordini { get; set; }
        public DbSet<DettaglioOrdine> DettagliOrdini { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


    }
}
