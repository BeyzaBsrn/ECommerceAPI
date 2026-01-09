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

            // Kayıt Ol
            group.MapPost("/register", async (CreateUserDto dto, IUserService service) =>
            {
                var result = await service.RegisterAsync(dto);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });
            // GİRİŞ YAP (LOGIN) - Token Al
            group.MapPost("/login", async (LoginUserDto dto, IUserService service) =>
            {
                var result = await service.LoginAsync(dto);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });
            // Listele
            group.MapGet("/", async (IUserService service) =>
            {
                var result = await service.GetAllUsersAsync();
                return Results.Ok(result);
            });

            // Tek Getir
            group.MapGet("/{id}", async (int id, IUserService service) =>
            {
                var result = await service.GetUserByIdAsync(id);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });

            // Güncelle
            group.MapPut("/", async (UpdateUserDto dto, IUserService service) =>
            {
                var result = await service.UpdateUserAsync(dto);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });

            // Sil
            group.MapDelete("/{id}", async (int id, IUserService service) =>
            {
                var result = await service.DeleteUserAsync(id);
                return result.Success ? Results.Ok(result) : Results.NotFound(result);
            });
        }
    }
}