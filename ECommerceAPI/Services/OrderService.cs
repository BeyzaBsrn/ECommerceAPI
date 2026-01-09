using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        // SİPARİŞ OLUŞTUR
        public async Task<ServiceResponse<Order>> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var response = new ServiceResponse<Order>();
            try
            {
                // Silinmiş kullanıcı sipariş veremez
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == orderDto.UserId && !u.IsDeleted);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Kullanıcı bulunamadı.";
                    return response;
                }

                var newOrder = new Order
                {
                    UserId = orderDto.UserId,
                    OrderDate = DateTime.Now,
                    IsDeleted = false,
                    OrderItems = new List<OrderItem>()
                };

                decimal totalAmount = 0;

                foreach (var item in orderDto.Items)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);

                    if (product == null)
                    {
                        response.Success = false;
                        response.Message = $"ID'si {item.ProductId} olan ürün bulunamadı.";
                        return response;
                    }

                    if (product.Stock < item.Quantity)
                    {
                        response.Success = false;
                        response.Message = $"{product.Name} ürününden yeterli stok yok. Kalan: {product.Stock}";
                        return response;
                    }

                    product.Stock -= item.Quantity; // Stoktan düş

                    var orderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        Price = product.Price
                    };

                    newOrder.OrderItems.Add(orderItem);
                    totalAmount += (product.Price * item.Quantity);
                }

                newOrder.TotalAmount = totalAmount;

                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();

                response.Data = newOrder;
                response.Success = true;
                response.Message = "Sipariş başarıyla oluşturuldu.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Hata: " + ex.Message;
            }
            return response;
        }

        // LİSTELE (Soft Delete Filtresi)
        public async Task<ServiceResponse<List<Order>>> GetAllOrdersAsync()
        {
            var response = new ServiceResponse<List<Order>>();
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => !o.IsDeleted) // Sadece silinmemişler
                .ToListAsync();

            response.Data = orders;
            response.Success = true;
            return response;
        }

        // 3. TEK GETİR
        public async Task<ServiceResponse<Order>> GetOrderByIdAsync(int id)
        {
            var response = new ServiceResponse<Order>();
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);

            if (order != null)
            {
                response.Data = order;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = "Sipariş bulunamadı.";
            }
            return response;
        }

        // 4. SİPARİŞ İPTAL ET (SOFT DELETE + STOK İADE)
        public async Task<ServiceResponse<bool>> DeleteOrderAsync(int id)
        {
            var response = new ServiceResponse<bool>();

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted); // Zaten silinmişse bulma

            if (order == null)
            {
                response.Success = false;
                response.Message = "Sipariş bulunamadı veya zaten silinmiş.";
                return response;
            }

            // Stokları geri iade et
            foreach (var item in order.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.Stock += item.Quantity;
                }
            }

            // ARTIK SİLMİYORUZ, UPDATE EDİYORUZ
            order.IsDeleted = true;
            order.UpdatedAt = DateTime.Now;

            // _context.Orders.Remove(order); <-- BU İPTAL
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Sipariş iptal edildi (Soft Delete) ve stoklar iade edildi.";
            return response;
        }
    }
}