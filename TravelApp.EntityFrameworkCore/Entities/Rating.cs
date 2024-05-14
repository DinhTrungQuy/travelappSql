

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAppAPI.Models
{
    [Table("Ratings")]
    public class Rating
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [ForeignKey("Places")]
        public string PlaceId { get; set; } = String.Empty;
        [ForeignKey("Users")]
        public string UserId { get; set; } = String.Empty;
        public int RatingValue { get; set; } = 0;
        public string Comment { get; set; } = String.Empty;
    }
}
