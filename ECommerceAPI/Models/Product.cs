using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int Stock { get; set; }

        // İlişki: Kategoriye bağlı
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // İlişki: Ürün özellikleri
        public List<ProductFeature> ProductFeatures { get; set; } = new List<ProductFeature>();
    }
}