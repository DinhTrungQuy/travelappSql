using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAppAPI.Models
{
    [Table("Dashboards")]
    public class Dashboard
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Date { get; set; } = DateTime.Today;
        public int Profit { get; set; } = 0;
        public int TotalUsers { get; set; } = 0;
        public int TotalPlaces { get; set; } = 0;
        public int TotalBookings { get; set; } = 0;
    }
}
