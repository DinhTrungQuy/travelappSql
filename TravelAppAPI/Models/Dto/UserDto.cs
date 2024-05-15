namespace TravelAppAPI.Models.Dto
{
    public class UserDto
    {
        public string Username { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Fullname { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;

        public IFormFile? Image { get; set; }
    }
}
