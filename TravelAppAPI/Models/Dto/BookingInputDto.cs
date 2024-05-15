using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TravelAppAPI.Model;

namespace TravelAppAPI.Models.Dto
{
    public class BookingInputDto
    {
        public string PlaceId { get; set; } = String.Empty;
        public int Quantity { get; set; } = 0;
        public int TotalPrice { get; set; } = 0;
        public int Status { get; set; } = 0;
        public DateTime CheckInTime { get; set; } = DateTime.Now;
        public DateTime CheckOutTime { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int Rating { get; set; } = 0;
    }
}

