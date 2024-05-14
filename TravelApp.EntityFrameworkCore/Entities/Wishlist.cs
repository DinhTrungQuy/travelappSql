

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAppAPI.Models
{
    [Table("Wishlists")]
    public class Wishlist
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [ForeignKey("Users")]
        public string UserId { get; set; } = String.Empty;
        [ForeignKey("Places")]
        public string PlaceId { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
