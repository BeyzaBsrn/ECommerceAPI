using ECommerceAPI.DTOs;
using ECommerceAPI.Services;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/orders").WithTags("Orders");

            // POST: Sipariş Ver
            group.MapPost("/", async (CreateOrderDto dto, IOrderService service) =>
            {
                var result = await service.CreateOrderAsync(dto);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            // GET: Tüm Siparişleri Listele
            group.MapGet("/", async (IOrderService service) =>
            {
                var result = await service.GetAllOrdersAsync();
                return Results.Ok(result);
            });

            // GET: ID ile Sipariş Getir
            group.MapGet("/{id}", async (int id, IOrderService service) =>
            {
                var result = await service.GetOrderByIdAsync(id);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });

            // DELETE: Sipariş Sil
            group.MapDelete("/{id}", async (int id, IOrderService service) =>
            {
                var result = await service.DeleteOrderAsync(id);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });
        }
    }
}