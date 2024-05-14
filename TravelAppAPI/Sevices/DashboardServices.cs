using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TravelAppAPI.Model;
using TravelAppAPI.Models;

namespace TravelAppAPI.Services
{
    public class DashboardServices
    {
        private readonly IMongoCollection<Dashboard> _dashboard;
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<Place> _place;
        private readonly IMongoCollection<Booking> _booking;

        public DashboardServices(IOptions<TravelAppDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _dashboard = database.GetCollection<Dashboard>(settings.Value.DashboardsCollectionName);
            _user = database.GetCollection<User>(settings.Value.UsersCollectionName);
            _place = database.GetCollection<Place>(settings.Value.PlacesCollectionName);
            _booking = database.GetCollection<Booking>(settings.Value.BookingsCollectionName);
        }
        public async Task<string> GetDashboard()
        {
            var dashboard = await _dashboard.Find(d => d.Date == DateTime.Today).FirstOrDefaultAsync();
            if (dashboard == null)
            {
                dashboard = new Dashboard();
                await _dashboard.InsertOneAsync(dashboard);
            }
            return dashboard.Id;
        }
        public async Task CreateAsync(Dashboard dashboard) => await _dashboard.InsertOneAsync(dashboard);
        public async Task<Dashboard> UpdateDashboard(Dashboard dashboard)
        {
            await _dashboard.ReplaceOneAsync(d => d.Id == dashboard.Id, dashboard);
            return dashboard;
        }
        public async Task<int> GetTotalUsers()
        {
            return (int)await _user.CountDocumentsAsync(d => true);
        }
        public async Task<int> GetTotalPlaces()
        {
            return (int)await _place.CountDocumentsAsync(d => true);
        }
        public async Task<int> GetTotalBookings()
        {
            return (int)await _booking.CountDocumentsAsync(d => true);
        }
        public int GetProfit()
        {
            var profit = _booking.AsQueryable().Where(b => b.Status == 2 | b.Status == 3).Sum(b => b.TotalPrice);
            return profit;
        }
    }
}
