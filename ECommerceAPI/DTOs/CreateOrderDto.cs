namespace ECommerceAPI.DTOs
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }
}