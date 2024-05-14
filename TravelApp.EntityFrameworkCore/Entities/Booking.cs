
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAppAPI.Models
{
    [Table("Bookings")]
    public class Booking
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [ForeignKey("Users")]
        public string UserId { get; set; } = String.Empty;
        [Required]
        [ForeignKey("Places")]
        public string PlaceId { get; set; } = String.Empty;
        public int Quantity { get; set; } = 0;
        [Required]
        public int TotalPrice { get; set; } = 0;
        public int Status { get; set; } = 0;
        public DateTime CheckInTime { get; set; } = DateTime.Now;
        public DateTime CheckOutTime { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int Rating { get; set; } = 0;
    }
}
