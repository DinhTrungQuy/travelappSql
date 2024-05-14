using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TravelAppAPI.Models;

namespace TravelAppAPI.Sevices
{
    public class BookingServices
    {
        private readonly IMongoCollection<Booking> _bookings;
        public BookingServices(IOptions<TravelAppDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _bookings = database.GetCollection<Booking>(settings.Value.BookingsCollectionName);
        }
        public async Task<List<Booking>> GetAsync()
        {
            return await _bookings.Find(booking => true).ToListAsync();
        }
        public async Task<Booking> GetAsync(string id)
        {
            return await _bookings.Find<Booking>(booking => booking.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Booking> CreateAsync(Booking booking)
        {
            await _bookings.InsertOneAsync(booking);
            return booking;
        }
        public async Task UpdateAsync(string id, Booking bookingIn)
        {
            await _bookings.ReplaceOneAsync(booking => booking.Id == id, bookingIn);

        }
        public async Task RemoveAsync(String id)
        {
            await _bookings.DeleteOneAsync(booking => booking.Id == id);
        }
        public async Task RemoveByPlaceIdAsync(string placeId)
        {
            await _bookings.DeleteManyAsync(booking => booking.PlaceId == placeId);
        }

    }
}
