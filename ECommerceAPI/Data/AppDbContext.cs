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
        public DbSet<ProductFeature> ProductFeatures { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  Kategori -> Ürün İlişkisi
            // Bir kategori silinirse, içinde ürün varsa izin verme (Restrict)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            //  Ürün -> Özellik İlişkisi
            // Ürün silinirse özellikleri de silinsin (Cascade)
            modelBuilder.Entity<ProductFeature>()
                .HasOne(pf => pf.Product)
                .WithMany(p => p.ProductFeatures)
                .HasForeignKey(pf => pf.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            //  User -> Order İlişkisi (YENİ)
            // Kullanıcı silinirse siparişleri de silinsin (Cascade)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //  Order -> OrderItem İlişkisi (YENİ)
            // Sipariş silinirse, siparişin detay satırları da silinsin
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            //  Product -> OrderItem İlişkisi (YENİ)
            //  Bir ürün silinmeye çalışılırsa, eğer o ürün daha önce satılmışsa (siparişi varsa) silmeye izin verme.
            // Geçmiş satış raporları bozulmasın diye Restrict yapıyoruz.
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany() // Product tarafında OrderItems listesi tutmadım
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}