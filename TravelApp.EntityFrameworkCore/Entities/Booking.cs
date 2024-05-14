
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelAppAPI.Model;

namespace TravelAppAPI.Models
{
    [Table("Bookings")]
    public class Booking
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [ForeignKey("UserId")]
        public User? User { get; set; }
        [Required]
        [ForeignKey("PlaceId")]
        public Place? Place { get; set; }
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
