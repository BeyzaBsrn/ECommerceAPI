using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<User>> RegisterAsync(CreateUserDto userDto)
        {
            var response = new ServiceResponse<User>();
            try
            {
                // 1. Kontrol: Bu email ile daha önce kayıt olunmuş mu?
                if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                {
                    response.Success = false;
                    response.Message = "Bu email adresi zaten kullanılıyor.";
                    return response;
                }

                // 2. Yeni Kullanıcıyı Hazırla
                var newUser = new User
                {
                    FullName = userDto.FullName,
                    Email = userDto.Email,
                    Password = userDto.Password, // Not: Gerçekte şifre hashlenmeli 
                    Address = userDto.Address,
                    CreatedAt = DateTime.Now
                };

                // 3. Veritabanına Ekledim
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                response.Data = newUser;
                response.Success = true;
                response.Message = "Kullanıcı başarıyla oluşturuldu.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Hata: " + ex.Message;
            }
            return response;
        }
    }
}