using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<CategoryDto>>> GetAllCategoriesAsync()
        {
            var response = new ServiceResponse<List<CategoryDto>>();
            try
            {
                var categories = await _context.Categories
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                    .ToListAsync();

                response.Data = categories;
                response.Success = true;
                response.Message = "Kategoriler listelendi.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<CategoryDto>> GetCategoryByIdAsync(int id)
        {
            var response = new ServiceResponse<CategoryDto>();
            try
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    response.Success = false;
                    response.Message = "Kategori bulunamadı.";
                }
                else
                {
                    response.Data = new CategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name
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

        public async Task<ServiceResponse<CategoryDto>> CreateCategoryAsync(CreateCategoryDto createDto)
        {
            var response = new ServiceResponse<CategoryDto>();
            try
            {
                var category = new Category
                {
                    Name = createDto.Name,
                    CreatedAt = DateTime.Now
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                response.Data = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                };
                response.Success = true;
                response.Message = "Kategori eklendi.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateCategoryAsync(UpdateCategoryDto updateDto)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var category = await _context.Categories.FindAsync(updateDto.Id);
                if (category == null)
                {
                    response.Success = false;
                    response.Message = "Kategori bulunamadı.";
                }
                else
                {
                    category.Name = updateDto.Name;
                    category.UpdatedAt = DateTime.Now;

                    await _context.SaveChangesAsync();

                    response.Data = true;
                    response.Success = true;
                    response.Message = "Kategori güncellendi.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteCategoryAsync(int id)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    response.Success = false;
                    response.Message = "Kategori bulunamadı.";
                }
                else
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();

                    response.Data = true;
                    response.Success = true;
                    response.Message = "Kategori silindi.";
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