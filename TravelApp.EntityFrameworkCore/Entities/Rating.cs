

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelAppAPI.Model;

namespace TravelAppAPI.Models
{
    [Table("Ratings")]
    public class Rating
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [ForeignKey("PlaceId")]
        public Place? Place { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public int RatingValue { get; set; } = 0;
        public string Comment { get; set; } = String.Empty;
    }
}
