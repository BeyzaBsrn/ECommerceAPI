using ECommerceAPI.Models;
using System.Text.Json.Serialization;

namespace ECommerceAPI.Models
{
    public class User : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Admin";
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // İlişki: Bir kullanıcının siparişleri olur
        [JsonIgnore]
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}