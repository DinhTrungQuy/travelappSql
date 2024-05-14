using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAppAPI.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Username { get; set; } = String.Empty;
        [Required]
        public string Password { get; set; } = String.Empty;
        public string Role { get; set; } = "user";
        public string Email { get; set; } = String.Empty;
        public string Fullname { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public string ImageUrl { get; set; } = String.Empty;

    }
}
