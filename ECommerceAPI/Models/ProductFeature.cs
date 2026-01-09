using ECommerceAPI.Models;

namespace ECommerceAPI.Models
{
    public class ProductFeature : BaseEntity
    {
        public string Key { get; set; } = string.Empty;   // Örn: Renk
        public string Value { get; set; } = string.Empty; // Örn: Kırmızı

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}