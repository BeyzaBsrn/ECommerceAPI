using ECommerceAPI.Models;

namespace ECommerceAPI.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}