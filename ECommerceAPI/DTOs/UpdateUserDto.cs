namespace ECommerceAPI.DTOs
{
    public class UpdateUserDto
    {
        public int Id { get; set; } // Hangi kullanıcı güncelleneecek
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
