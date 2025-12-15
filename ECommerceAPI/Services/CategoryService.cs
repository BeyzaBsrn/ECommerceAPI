using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        // Veritabanı bağlantısını buraya çağırıyoruz (Dependency Injection)
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            // Veritabanındaki Category'leri al -> CategoryDto'ya çevir -> Listele
            var categories = await _context.Categories
                .Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return categories;
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto)
        {
            // DTO'dan gelen veriyi Entity'e çevir
            var newCategory = new Category
            {
                Name = createDto.Name
            };

            // Veritabanına ekle
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            // Geriye oluşturulanı döndür
            return new CategoryDto
            {
                Id = newCategory.Id,
                Name = newCategory.Name
            };
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateDto)
        {
            var category = await _context.Categories.FindAsync(updateDto.Id);
            if (category == null) return; // Veya hata fırlatılabilir

            category.Name = updateDto.Name;
            category.UpdatedAt = DateTime.Now; // Güncelleme tarihini atadık

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}