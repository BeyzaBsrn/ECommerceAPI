using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace ECommerceAPI.Models
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        [JsonIgnore]

        public Order? Order { get; set; }

        public int ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }

        public int Quantity { get; set; } //  adet
        [Precision(18, 2)]
        public decimal Price { get; set; } // Satış Fiyatı
    }
}