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
                // Kontrol: Bu email ile daha önce kayıt olunmuş mu?
                if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                {
                    response.Success = false;
                    response.Message = "Bu email adresi zaten kullanılıyor.";
                    return response;
                }

                // Yeni Kullanıcıyı Hazırla
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

        // TÜM KULLANICILARI GETİR (GET)
        public async Task<ServiceResponse<List<User>>> GetAllUsersAsync()
        {
            var response = new ServiceResponse<List<User>>();
            var users = await _context.Users.ToListAsync();
            response.Data = users;
            response.Success = true;
            return response;
        }

        // ID İLE TEK KULLANICI GETİR (GET BY ID)
        public async Task<ServiceResponse<User>> GetUserByIdAsync(int id)
        {
            var response = new ServiceResponse<User>();
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                response.Success = false;
                response.Message = "Kullanıcı bulunamadı.";
            }
            else
            {
                response.Data = user;
                response.Success = true;
            }
            return response;
        }

        // KULLANICI GÜNCELLE (PUT)
        public async Task<ServiceResponse<User>> UpdateUserAsync(UpdateUserDto userDto)
        {
            var response = new ServiceResponse<User>();
            var user = await _context.Users.FindAsync(userDto.Id);

            if (user == null)
            {
                response.Success = false;
                response.Message = "Güncellenecek kullanıcı bulunamadı.";
                return response;
            }

            // Verileri güncelle
            user.FullName = userDto.FullName;
            user.Email = userDto.Email;
            user.Address = userDto.Address;

            await _context.SaveChangesAsync();

            response.Data = user;
            response.Success = true;
            response.Message = "Kullanıcı bilgileri güncellendi.";
            return response;
        }

        // KULLANICI SİL (DELETE)
        public async Task<ServiceResponse<bool>> DeleteUserAsync(int id)
        {
            var response = new ServiceResponse<bool>();
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                response.Success = false;
                response.Message = "Kullanıcı bulunamadı.";
            }
            else
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Kullanıcı silindi.";
            }
            return response;
        }
    }
}