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
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Review> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Maglia)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MagliaId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);
        }


    }
}
