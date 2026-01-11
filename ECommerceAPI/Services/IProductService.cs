using ECommerceAPI.DTOs;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<List<ProductDto>>> GetAllProductsAsync();
        Task<ServiceResponse<ProductDto>> GetProductByIdAsync(int id);
        Task<ServiceResponse<ProductDto>> CreateProductAsync(CreateProductDto createDto);
        Task<ServiceResponse<bool>> UpdateProductAsync(UpdateProductDto updateDto);
        Task<ServiceResponse<bool>> DeleteProductAsync(int id);
    }
}