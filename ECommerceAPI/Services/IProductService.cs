using ECommerceAPI.DTOs;

namespace ECommerceAPI.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto createDto);
        Task UpdateProductAsync(UpdateProductDto updateDto);
        Task DeleteProductAsync(int id);
    }
}