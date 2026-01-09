using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Controllers
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/products").WithTags("Products");

            // 1. LİSTELE
            group.MapGet("/", async (IProductService service) =>
            {
                var result = await service.GetAllProductsAsync();
                return Results.Ok(result);
            });

            // 2. TEK GETİR
            group.MapGet("/{id}", async (int id, IProductService service) =>
            {
                var result = await service.GetProductByIdAsync(id);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });

            // 3. EKLE (İsim CreateProductAsync oldu)
            group.MapPost("/", async (CreateProductDto dto, IProductService service) =>
            {
                var result = await service.CreateProductAsync(dto);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            // 4. GÜNCELLE
            group.MapPut("/", async (UpdateProductDto dto, IProductService service) =>
            {
                var result = await service.UpdateProductAsync(dto);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });

            // 5. SİL
            group.MapDelete("/{id}", async (int id, IProductService service) =>
            {
                var result = await service.DeleteProductAsync(id);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });

            // 🔥 .NET 9 ÖZEL ENDPOINT: İSTATİSTİK (CountBy Kullanımı)
            // Hocaya gösterilecek kısım burası
            group.MapGet("/stats", async (AppDbContext context) =>
            {
                var products = await context.Products.Include(p => p.Category).ToListAsync();

                // .NET 9 İLE GELEN YENİ ÖZELLİK: CountBy
                var stats = products.CountBy(p => p.Category?.Name ?? "Diğer");

                return Results.Ok(new ServiceResponse<object>
                {
                    Success = true,
                    Message = "Kategori bazlı ürün dağılımı (.NET 9 CountBy ile hesaplandı).",
                    Data = stats
                });
            });
        }
    }
}