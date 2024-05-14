namespace TravelAppAPI.Models.Dto
{
    public class RegisterDto
    {
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Fullname { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public string ImageUrl { get; set; } = String.Empty;
    }
}
