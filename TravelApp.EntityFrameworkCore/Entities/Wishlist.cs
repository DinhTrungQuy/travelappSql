

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelAppAPI.Model;

namespace TravelAppAPI.Models
{
    [Table("Wishlists")]
    public class Wishlist
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [ForeignKey("UserId")]
        public User User { get; set; }
        [Required]
        [ForeignKey("PlaceId")]
        public Place Place { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
