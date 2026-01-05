using ECommerceAPI.DTOs;
using ECommerceAPI.Services;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/products").WithTags("Products");

            group.MapGet("/", async (IProductService service) =>
            {
                var result = await service.GetAllProductsAsync();
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            group.MapGet("/{id}", async (int id, IProductService service) =>
            {
                var result = await service.GetProductByIdAsync(id);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });

            group.MapPost("/", async (CreateProductDto dto, IProductService service) =>
            {
                var result = await service.CreateProductAsync(dto);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            group.MapPut("/", async (UpdateProductDto dto, IProductService service) =>
            {
                var result = await service.UpdateProductAsync(dto);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            group.MapDelete("/{id}", async (int id, IProductService service) =>
            {
                var result = await service.DeleteProductAsync(id);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });
        }
    }
}