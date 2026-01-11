using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        // ProductFeatures tablosunu sildiğimiz için buradaki DbSet kalsa da sorun olmaz ama silebilirsin de.
        // public DbSet<ProductFeature> ProductFeatures { get; set; } 

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // İLİŞKİLER (Aynı kalacak)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            //  SEED DATA 

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Spor Ayakkabı", IsDeleted = false, CreatedAt = DateTime.Now },
                new Category { Id = 2, Name = "Bot & Çizme", IsDeleted = false, CreatedAt = DateTime.Now },
                new Category { Id = 3, Name = "Topuklu Ayakkabı", IsDeleted = false, CreatedAt = DateTime.Now }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Nike Air Zoom",
                    Description = "Koşu ve yürüyüş için ideal, rahat taban.",
                    Price = 3500,
                    Stock = 20,
                    CategoryId = 1, // Spor
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Id = 2,
                    Name = "Adidas Superstar",
                    Description = "Klasik beyaz sneaker, günlük kullanım.",
                    Price = 2800,
                    Stock = 15,
                    CategoryId = 1, // Spor
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Id = 3,
                    Name = "Harley Davidson Bot",
                    Description = "Su geçirmez, hakiki deri kışlık bot.",
                    Price = 4500,
                    Stock = 50,
                    CategoryId = 2, // Bot
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Id = 4,
                    Name = "Stiletto Siyah",
                    Description = "10 cm topuklu, klasik siyah.",
                    Price = 1200,
                    Stock = 10,
                    CategoryId = 3, // Topuklu
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                }
            );

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // PendingModelChanges uyarısını yoksaymak için ekledim
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));

            base.OnConfiguring(optionsBuilder);
        }
    }
}