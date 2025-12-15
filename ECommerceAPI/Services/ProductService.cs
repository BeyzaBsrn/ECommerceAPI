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

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            // Include: Ürünü çekerken Kategori tablosunu da birleştir (SQL JOIN)
            var products = await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    // Eğer kategori silinmişse veya yoksa "Kategorisiz" yazsın
                    CategoryName = p.Category != null ? p.Category.Name : "Kategorisiz"
                })
                .ToListAsync();

            return products;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryName = product.Category != null ? product.Category.Name : "Kategorisiz"
            };
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createDto)
        {
            var newProduct = new Product
            {
                Name = createDto.Name,
                Price = createDto.Price,
                Stock = createDto.Stock,
                CategoryId = createDto.CategoryId
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            // Geriye dönmek için tekrar DTO oluşturuyoruz
            return new ProductDto
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Price = newProduct.Price,
                Stock = newProduct.Stock,
                CategoryName = "" // Yeni eklendiğinde ismini tekrar sorgulamaya gerek yok şimdilik
            };
        }

        public async Task UpdateProductAsync(UpdateProductDto updateDto)
        {
            var product = await _context.Products.FindAsync(updateDto.Id);
            if (product == null) return;

            product.Name = updateDto.Name;
            product.Price = updateDto.Price;
            product.Stock = updateDto.Stock;
            product.CategoryId = updateDto.CategoryId;
            product.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}