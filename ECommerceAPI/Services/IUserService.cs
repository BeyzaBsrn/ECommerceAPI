using ECommerceAPI.DTOs;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface IUserService
    {
        // Kullanıcı kaydı için metod tanımı yapılıyo
        Task<ServiceResponse<User>> RegisterAsync(CreateUserDto userDto);
    }
}