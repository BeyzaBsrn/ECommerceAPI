using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductDto>>> GetAllProductsAsync()
        {
            var response = new ServiceResponse<List<ProductDto>>();
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Stock = p.Stock,
                        CategoryName = p.Category != null ? p.Category.Name : "Kategorisiz"
                    })
                    .ToListAsync();

                response.Data = products;
                response.Success = true;
                response.Message = "İşlem başarılı.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<ProductDto>> GetProductByIdAsync(int id)
        {
            var response = new ServiceResponse<ProductDto>();
            try
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    response.Success = false;
                    response.Message = "Ürün bulunamadı.";
                }
                else
                {
                    response.Data = new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Stock = product.Stock,
                        CategoryName = product.Category != null ? product.Category.Name : "Kategorisiz"
                    };
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<ProductDto>> CreateProductAsync(CreateProductDto createDto)
        {
            var response = new ServiceResponse<ProductDto>();
            try
            {
                var newProduct = new Product
                {
                    Name = createDto.Name,
                    Price = createDto.Price,
                    Stock = createDto.Stock,
                    CategoryId = createDto.CategoryId,
                    CreatedAt = DateTime.Now
                };

                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();

                response.Data = new ProductDto
                {
                    Id = newProduct.Id,
                    Name = newProduct.Name,
                    Price = newProduct.Price,
                    Stock = newProduct.Stock,
                    CategoryName = ""
                };
                response.Success = true;
                response.Message = "Ürün başarıyla eklendi.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateProductAsync(UpdateProductDto updateDto)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var product = await _context.Products.FindAsync(updateDto.Id);
                if (product == null)
                {
                    response.Success = false;
                    response.Message = "Güncellenecek ürün bulunamadı.";
                }
                else
                {
                    product.Name = updateDto.Name;
                    product.Price = updateDto.Price;
                    product.Stock = updateDto.Stock;
                    product.CategoryId = updateDto.CategoryId;
                    product.UpdatedAt = DateTime.Now;

                    await _context.SaveChangesAsync();

                    response.Data = true;
                    response.Success = true;
                    response.Message = "Ürün güncellendi.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteProductAsync(int id)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    response.Success = false;
                    response.Message = "Silinecek ürün bulunamadı.";
                }
                else
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();

                    response.Data = true;
                    response.Success = true;
                    response.Message = "Ürün silindi.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}