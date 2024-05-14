using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppAPI.Model;
using TravelAppAPI.Models;

namespace TravelApp.EntityFrameworkCore
{
    public class TravelAppDbContext :DbContext
    {
        public TravelAppDbContext(DbContextOptions<TravelAppDbContext> options) : base(options)
        {


        }
        public virtual DbSet<Booking> bookings { get; set; }
        public virtual DbSet<Dashboard> dashboards { get; set; }
        public virtual DbSet<Place> places { get; set; }
        public virtual DbSet<User> users { get; set; }
        public virtual DbSet<Wishlist> wishlists { get; set; }
        public virtual DbSet<Rating> ratings { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
                optionsBuilder.UseSqlServer("Server=BNC-SERVER; Database=Speak;user id=sa; password=Bnc@1612;TrustServerCertificate=True");
           
        }
    }
}
