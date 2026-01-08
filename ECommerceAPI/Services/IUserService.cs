// Services/IUserService.cs
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<User>> RegisterAsync(CreateUserDto userDto);
        Task<ServiceResponse<List<User>>> GetAllUsersAsync();
        Task<ServiceResponse<User>> GetUserByIdAsync(int id);
        Task<ServiceResponse<User>> UpdateUserAsync(UpdateUserDto userDto); 
        Task<ServiceResponse<bool>> DeleteUserAsync(int id);
    }
}