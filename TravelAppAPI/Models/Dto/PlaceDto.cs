namespace TravelAppAPI.Models.Dto
{
    public class PlaceDto
    {

        public int DurationDays { get; set; } = 0;
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string Location { get; set; } = String.Empty;
        public int Price { get; set; } = 0;
        public bool Popular { get; set; } = false;
        public bool Recommended { get; set; } = false;
        public int Direction { get; set; } = 0;
        public IFormFile? Image { get; set; }
    }
}
