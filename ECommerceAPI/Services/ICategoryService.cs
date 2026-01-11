using ECommerceAPI.DTOs;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<CategoryDto>>> GetAllCategoriesAsync();
        Task<ServiceResponse<CategoryDto>> GetCategoryByIdAsync(int id);
        Task<ServiceResponse<CategoryDto>> CreateCategoryAsync(CreateCategoryDto createDto);
        Task<ServiceResponse<bool>> UpdateCategoryAsync(UpdateCategoryDto updateDto);
        Task<ServiceResponse<bool>> DeleteCategoryAsync(int id);
    }
}