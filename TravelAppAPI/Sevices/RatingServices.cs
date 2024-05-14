using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TravelAppAPI.Models;

namespace TravelAppAPI.Sevices
{
    public class RatingServices
    {
        private readonly IMongoCollection<Rating> _ratings;
        public RatingServices(IOptions<TravelAppDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _ratings = database.GetCollection<Rating>(settings.Value.RatingsCollectionName);
        }
        public async Task<List<Rating>> GetAsync() => await _ratings.Find(rating => true).ToListAsync();
        public async Task<List<Rating>> GetByPlaceIdAsync(string placeId) => await _ratings.Find<Rating>(rating => rating.PlaceId == placeId).ToListAsync();
        public async Task<Rating> InsertAsync(Rating rating)
        {
            await _ratings.InsertOneAsync(rating);
            return rating;
        }
        //public async Task UpdateAsync(string id, Rating ratingIn) => await _ratings.ReplaceOneAsync(rating => rating.Id == id, ratingIn);
        public async Task RemoveAsync(Rating ratingIn) => await _ratings.DeleteOneAsync(rating => rating.Id == ratingIn.Id);

    }
}
