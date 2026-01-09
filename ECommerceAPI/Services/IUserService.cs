using ECommerceAPI.DTOs;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<UserResponseDto>> RegisterAsync(CreateUserDto userDto);

        Task<ServiceResponse<string>> LoginAsync(LoginUserDto loginDto);
        Task<ServiceResponse<List<UserResponseDto>>> GetAllUsersAsync();
        Task<ServiceResponse<UserResponseDto>> GetUserByIdAsync(int id);
        Task<ServiceResponse<UserResponseDto>> UpdateUserAsync(UpdateUserDto userDto);
        Task<ServiceResponse<bool>> DeleteUserAsync(int id);
    }
}