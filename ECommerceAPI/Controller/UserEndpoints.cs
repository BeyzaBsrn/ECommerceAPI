using ECommerceAPI.DTOs;
using ECommerceAPI.Services;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/users").WithTags("Users");

            // Kayıt Ol (POST)
            group.MapPost("/register", async (CreateUserDto dto, IUserService service) =>
            {
                var result = await service.RegisterAsync(dto);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            // Listele (GET)
            group.MapGet("/", async (IUserService service) =>
            {
                return Results.Ok(await service.GetAllUsersAsync());
            });

            // Tek Getir (GET {id})
            group.MapGet("/{id}", async (int id, IUserService service) =>
            {
                var result = await service.GetUserByIdAsync(id);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });

            // Güncelle (PUT)
            group.MapPut("/", async (UpdateUserDto dto, IUserService service) =>
            {
                var result = await service.UpdateUserAsync(dto);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });

            // Sil (DELETE)
            group.MapDelete("/{id}", async (int id, IUserService service) =>
            {
                var result = await service.DeleteUserAsync(id);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });
        }
    }
}