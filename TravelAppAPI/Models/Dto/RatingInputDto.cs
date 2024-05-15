namespace TravelAppAPI.Models.Dto
{
    public class RatingInputDto
    {
        public string UserId { get; set; } = String.Empty;
        public string PlaceId { get; set; } = String.Empty;
        public int RatingValue { get; set; }
        public string Comment { get; set; } = String.Empty;
    }
}
