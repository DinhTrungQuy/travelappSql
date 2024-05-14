
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAppAPI.Model
{
    [Table("Places")]
    public class Place
    {
        [Key]
        public string PlaceId { get; set; } = Guid.NewGuid().ToString();
        public int DurationDays { get; set; } = 0;
        [Required]
        public string Name { get; set; } = String.Empty;
        [Required]
        public string Description { get; set; } = String.Empty;
        public string ImageUrl { get; set; } = String.Empty;
        public string Rating { get; set; } = String.Empty;
        public string Location { get; set; } = String.Empty;
        [Required]
        public int Price { get; set; } = 0;
        public bool Popular { get; set; } = false;
        public bool Recommended { get; set; } = false;
        public int Direction { get; set; } = 0;
    }
}
