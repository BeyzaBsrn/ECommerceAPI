using ECommerceAPI.Models;

namespace ECommerceAPI.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        // İlişki: Kategori silinirse ürünlere ne olacağını Data katmanında ayarlarız, şimdilik listeyi koyalım.
        public List<Product> Products { get; set; } = new List<Product>();
    }
}