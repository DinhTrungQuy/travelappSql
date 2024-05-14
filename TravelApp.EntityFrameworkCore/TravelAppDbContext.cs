using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TravelAppAPI.Model;
using TravelAppAPI.Models;

namespace TravelApp.EntityFrameworkCore
{
    public class TravelAppDbContext : DbContext
    {
        public TravelAppDbContext(DbContextOptions<TravelAppDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Dashboard> Dashboards { get; set; }
        public virtual DbSet<Place> Places { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Wishlist> Wishlists { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    IConfigurationRoot config = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json")
                       .Build();
                    string connString = config.GetConnectionString("DefaultConnection") ?? "Server=DESKTOP-50I9PSS\\SQLEXPRESS; Database=Speak;user id=sa; password=123456;TrustServerCertificate=True";
                    optionsBuilder.UseSqlServer(connString); //Or whatever DB you are using
                }
            }
        }
    }
}
