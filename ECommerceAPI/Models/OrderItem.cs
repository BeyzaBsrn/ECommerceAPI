using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Models
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }

        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; } //  adet
        [Precision(18, 2)]
        public decimal Price { get; set; } // Satış Fiyatı
    }
}