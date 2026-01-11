using ECommerceAPI.DTOs;
using ECommerceAPI.Services;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/categories").WithTags("Categories");

            group.MapGet("/", async (ICategoryService service) =>
            {
                var result = await service.GetAllCategoriesAsync();
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            group.MapGet("/{id}", async (int id, ICategoryService service) =>
            {
                var result = await service.GetCategoryByIdAsync(id);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });

            group.MapPost("/", async (CreateCategoryDto dto, ICategoryService service) =>
            {
                var result = await service.CreateCategoryAsync(dto);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            group.MapPut("/", async (UpdateCategoryDto dto, ICategoryService service) =>
            {
                var result = await service.UpdateCategoryAsync(dto);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            group.MapDelete("/{id}", async (int id, ICategoryService service) =>
            {
                var result = await service.DeleteCategoryAsync(id);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });
        }
    }
}