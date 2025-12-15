using ECommerceAPI.DTOs;

namespace ECommerceAPI.Services
{
    public interface ICategoryService
    {
        // Tüm kategorileri getir
        Task<List<CategoryDto>> GetAllCategoriesAsync();

        // Id'ye göre tek bir kategori getir
        Task<CategoryDto?> GetCategoryByIdAsync(int id);

        // Yeni kategori ekle (Geriye eklenen kategoriyi döndürür)
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto);

        // Kategoriyi güncelle
        Task UpdateCategoryAsync(UpdateCategoryDto updateDto);

        // Kategoriyi sil
        Task DeleteCategoryAsync(int id);
    }
}