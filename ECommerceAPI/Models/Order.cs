using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Models
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [Precision(18, 2)]
        public decimal TotalAmount { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; } 

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); //Siparişin içindeki ürünler(ayakkabılar)
    }
}
