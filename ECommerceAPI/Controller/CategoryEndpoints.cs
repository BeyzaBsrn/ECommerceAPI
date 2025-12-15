using ECommerceAPI.DTOs;
using ECommerceAPI.Services;

namespace ECommerceAPI.Controllers
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this WebApplication app)
        {
            // Tüm kategori işlemleri "/api/categories" altında toplanacak
            var group = app.MapGroup("/api/categories").WithTags("Categories");

            // GET: Tümünü Listele
            group.MapGet("/", async (ICategoryService service) =>
            {
                var result = await service.GetAllCategoriesAsync();
                return Results.Ok(result);
            });

            // GET: Id'ye göre getir
            group.MapGet("/{id}", async (int id, ICategoryService service) =>
            {
                var result = await service.GetCategoryByIdAsync(id);
                if (result == null) return Results.NotFound("Kategori bulunamadı.");
                return Results.Ok(result);
            });

            // POST: Yeni Ekle
            group.MapPost("/", async (CreateCategoryDto dto, ICategoryService service) =>
            {
                var result = await service.CreateCategoryAsync(dto);
                return Results.Created($"/api/categories/{result.Id}", result);
            });

            // PUT: Güncelle
            group.MapPut("/", async (UpdateCategoryDto dto, ICategoryService service) =>
            {
                await service.UpdateCategoryAsync(dto);
                return Results.Ok("Güncellendi.");
            });

            // DELETE: Sil
            group.MapDelete("/{id}", async (int id, ICategoryService service) =>
            {
                await service.DeleteCategoryAsync(id);
                return Results.Ok("Silindi.");
            });
        }
    }
}
