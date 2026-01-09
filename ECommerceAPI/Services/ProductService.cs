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
        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "Kategorisiz" // Null kontrolü
            };
        }

        // LİSTELE (List<ProductDto> döner)
        public async Task<ServiceResponse<List<ProductDto>>> GetAllProductsAsync()
        {
            var response = new ServiceResponse<List<ProductDto>>();

            var products = await _context.Products
                                         .Include(p => p.Category)
                                         .ToListAsync();

            response.Data = products.Select(p => MapToDto(p)).ToList();
            response.Success = true;
            return response;
        }

        // TEK GETİR
        public async Task<ServiceResponse<ProductDto>> GetProductByIdAsync(int id)
        {
            var response = new ServiceResponse<ProductDto>();
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
                response.Data = MapToDto(product);
                response.Success = true;
            }
            return response;
        }

        // EKLE 
        public async Task<ServiceResponse<ProductDto>> CreateProductAsync(CreateProductDto createDto)
        {
            var response = new ServiceResponse<ProductDto>();

            var newProduct = new Product
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                Stock = createDto.Stock,
                CategoryId = createDto.CategoryId
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            // Eklenen ürünü DTO olarak geri döndür
            // İlişki adını görebilmek için kategoriyi tekrar yükleyebiliriz veya basitçe DTO döneriz
            response.Data = MapToDto(newProduct);
            response.Success = true;
            response.Message = "Ürün başarıyla eklendi.";
            return response;
        }

        //  GÜNCELLE 
        public async Task<ServiceResponse<bool>> UpdateProductAsync(UpdateProductDto updateDto)
        {
            var response = new ServiceResponse<bool>();
            var product = await _context.Products.FindAsync(updateDto.Id);

            if (product == null)
            {
                response.Success = false;
                response.Message = "Ürün bulunamadı.";
                return response;
            }

            product.Name = updateDto.Name;
            product.Price = updateDto.Price;
            product.Stock = updateDto.Stock;
            product.Description = updateDto.Description;
            product.CategoryId = updateDto.CategoryId;

            await _context.SaveChangesAsync();

            response.Data = true;
            response.Success = true;
            response.Message = "Ürün güncellendi.";
            return response;
        }

        // SİL 
        public async Task<ServiceResponse<bool>> DeleteProductAsync(int id)
        {
            var response = new ServiceResponse<bool>();
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                response.Success = false;
                response.Message = "Ürün bulunamadı.";
                return response;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            response.Data = true;
            response.Success = true;
            response.Message = "Ürün silindi.";
            return response;
        }
    }
}