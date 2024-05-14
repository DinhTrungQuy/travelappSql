using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TravelAppAPI.Model;
using TravelAppAPI.Models;

namespace TravelAppAPI.Sevices
{
    public class PlaceServices
    {
        private readonly IMongoCollection<Place> _places;
        public PlaceServices(IOptions<TravelAppDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _places = database.GetCollection<Place>(settings.Value.PlacesCollectionName);
        }
        public async Task<List<Place>> GetAsync() => await _places.Find(place => true).ToListAsync();
        public async Task<Place> GetAsync(string id) => await _places.Find<Place>(place => place.Id == id).FirstOrDefaultAsync();
        public async Task<Place> CreateAsync(Place place)
        {
            await _places.InsertOneAsync(place);
            return place;
        }
        public async Task UpdateAsync(string id, Place placeIn) => await _places.ReplaceOneAsync(place => place.Id == id, placeIn);
        public async Task UpdateRating(string placeId, double ratingValue)
        {
            var place = await GetAsync(placeId);
            place.Rating = ratingValue.ToString();
            await UpdateAsync(placeId, place);
        }
        public async Task RemoveAsync(Place placeIn) => await _places.DeleteOneAsync(place => place.Id == placeIn.Id);
        public async Task RemoveAsync(string id) => await _places.DeleteOneAsync(place => place.Id == id);
    }
}
