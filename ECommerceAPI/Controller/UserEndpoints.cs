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

            // POST: /api/users/register
            group.MapPost("/register", async (CreateUserDto dto, IUserService service) =>
            {
                var result = await service.RegisterAsync(dto);

                if (result.Success)
                {
                    return Results.Ok(result); // 200 OK
                }
                else
                {
                    return Results.BadRequest(result); // 400 Hata
                }
            });
        }
    }
}
