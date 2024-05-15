using Microsoft.EntityFrameworkCore;
using TravelApp.EntityFrameworkCore;
using TravelAppAPI.Models;

namespace TravelAppAPI.Services
{
    public class BookingServices
    {
        private readonly TravelAppDbContext _context;

        public BookingServices(TravelAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetAsync()
        {
            return await _context.Bookings.Include(b => b.User).Include(b => b.Place).ToListAsync();
        }
        public async Task<List<Booking>> GetByUserIdAsync(string userId)
        {
            return await _context.Bookings.Include(b => b.User).Include(b => b.Place).Where(b => b.User.UserId == userId).ToListAsync();
        }

        public async Task<Booking> GetAsync(string id)
        {
            return await _context.Bookings.Include(b => b.User).Include(b => b.Place).Where(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Booking> CreateAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task UpdateAsync(string id, Booking bookingIn)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Entry(booking).CurrentValues.SetValues(bookingIn);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(string id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveByPlaceIdAsync(string placeId)
        {
            var bookings = _context.Bookings.Where(b => b.Place.PlaceId == placeId);
            _context.Bookings.RemoveRange(bookings);
            await _context.SaveChangesAsync();
        }
    }
}
