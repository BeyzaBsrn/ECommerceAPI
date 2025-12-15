using ECommerceAPI.DTOs;
using ECommerceAPI.Services;

namespace ECommerceAPI.Controllers
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/products").WithTags("Products");

            // GET: Tümünü Listele
            group.MapGet("/", async (IProductService service) =>
            {
                var result = await service.GetAllProductsAsync();
                return Results.Ok(result);
            });

            // GET: Id'ye göre getir
            group.MapGet("/{id}", async (int id, IProductService service) =>
            {
                var result = await service.GetProductByIdAsync(id);
                if (result == null) return Results.NotFound("Ürün bulunamadı.");
                return Results.Ok(result);
            });

            // POST: Yeni Ekle
            group.MapPost("/", async (CreateProductDto dto, IProductService service) =>
            {
                var result = await service.CreateProductAsync(dto);
                return Results.Created($"/api/products/{result.Id}", result);
            });

            // PUT: Güncelle
            group.MapPut("/", async (UpdateProductDto dto, IProductService service) =>
            {
                await service.UpdateProductAsync(dto);
                return Results.Ok("Güncellendi.");
            });

            // DELETE: Sil
            group.MapDelete("/{id}", async (int id, IProductService service) =>
            {
                await service.DeleteProductAsync(id);
                return Results.Ok("Silindi.");
            });
        }
    }
}