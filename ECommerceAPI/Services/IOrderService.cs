using ECommerceAPI.DTOs;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface IOrderService
    {
        Task<ServiceResponse<Order>> CreateOrderAsync(CreateOrderDto orderDto);
        Task<ServiceResponse<List<Order>>> GetAllOrdersAsync(); // Tümünü Getir
        Task<ServiceResponse<Order>> GetOrderByIdAsync(int id); // Tek Getir
        Task<ServiceResponse<bool>> DeleteOrderAsync(int id);   // Sil (Stok İadeli)
    }
}