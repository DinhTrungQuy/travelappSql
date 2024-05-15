namespace TravelAppAPI.Models.Dto
{
    public class BookingOutputDto
    {
        public string Id { get; set; } = String.Empty;
        public string UserId { get; set; } = String.Empty;
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
