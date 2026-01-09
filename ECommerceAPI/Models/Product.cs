using System.Text.Json.Serialization;

namespace ECommerceAPI.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public int CategoryId { get; set; }

        [JsonIgnore] // Döngü olmaması için ekledim
        public Category? Category { get; set; }
    }
}