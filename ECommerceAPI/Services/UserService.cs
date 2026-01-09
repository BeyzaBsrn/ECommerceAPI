using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; 
using System.IdentityModel.Tokens.Jwt; 
using System.Security.Claims; 
using System.Text; 

namespace ECommerceAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration; 

        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> LoginAsync(LoginUserDto loginDto)
        {
            var response = new ServiceResponse<string>();

            // Kullanıcıyı Bul
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email && !u.IsDeleted);

            // Kullanıcı Yoksa veya Şifre Yanlışsa
            if (user == null || user.Password != loginDto.Password) 
            {
                response.Success = false;
                response.Message = "Email veya şifre hatalı.";
                return response;
            }

            // JWT Oluşturma
            response.Data = CreateToken(user);
            response.Success = true;
            response.Message = "Giriş başarılı. Token oluşturuldu.";

            return response;
        }

        // Token Oluşturan Yardımcı Metot
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role) // Admin/User rolü
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JwtSettings:SecretKey").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1), // Token 1 gün geçerli
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token); 
        }

        private UserResponseDto MapToDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                Role = user.Role
            };
        }

        public async Task<ServiceResponse<UserResponseDto>> RegisterAsync(CreateUserDto userDto)
        {
            var response = new ServiceResponse<UserResponseDto>();
            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                {
                    response.Success = false;
                    response.Message = "Bu email adresi zaten kullanılıyor.";
                    return response;
                }

                var newUser = new User
                {
                    FullName = userDto.FullName,
                    Email = userDto.Email,
                    Password = userDto.Password,
                    Address = userDto.Address,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                response.Data = MapToDto(newUser);
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

        public async Task<ServiceResponse<List<UserResponseDto>>> GetAllUsersAsync()
        {
            var response = new ServiceResponse<List<UserResponseDto>>();
            var users = await _context.Users.Where(u => !u.IsDeleted).ToListAsync();
            response.Data = users.Select(user => MapToDto(user)).ToList();
            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<UserResponseDto>> GetUserByIdAsync(int id)
        {
            var response = new ServiceResponse<UserResponseDto>();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            if (user == null) { response.Success = false; response.Message = "Kullanıcı bulunamadı."; }
            else { response.Data = MapToDto(user); response.Success = true; }
            return response;
        }

        public async Task<ServiceResponse<UserResponseDto>> UpdateUserAsync(UpdateUserDto userDto)
        {
            var response = new ServiceResponse<UserResponseDto>();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userDto.Id && !u.IsDeleted);

            if (user == null)
            {
                response.Success = false;
                response.Message = "Kullanıcı bulunamadı.";
                return response;
            }

            user.FullName = userDto.FullName;
            user.Email = userDto.Email;
            user.Address = userDto.Address;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            response.Data = MapToDto(user);
            response.Success = true;
            response.Message = "Kullanıcı güncellendi.";
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteUserAsync(int id)
        {
            var response = new ServiceResponse<bool>();
            var user = await _context.Users.FindAsync(id);

            if (user == null || user.IsDeleted) { response.Success = false; response.Message = "Kullanıcı bulunamadı."; }
            else
            {
                user.IsDeleted = true;
                user.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Kullanıcı silindi (Soft Delete).";
            }
            return response;
        }
    }
}