using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TravelAppAPI.Models;

namespace TravelAppAPI.Sevices
{
    public class WishlistServices
    {
        private readonly IMongoCollection<Wishlist> _wishlist;
        public WishlistServices(IOptions<TravelAppDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _wishlist = database.GetCollection<Wishlist>(settings.Value.WishlistsCollectionName);
        }
        public async Task<List<Wishlist>> GetAsync()
        {
            return await _wishlist.Find(wishlist => true).ToListAsync();
        }
        public async Task<List<Wishlist>> GetAsync(string userId)
        {
            return await _wishlist.Find<Wishlist>(wishlist => wishlist.UserId == userId).ToListAsync();
        }
        public async Task<Wishlist> CheckExist(string userId, string placeId)
        {
            return await _wishlist.Find<Wishlist>(wishlist => wishlist.UserId == userId && wishlist.PlaceId == placeId).FirstOrDefaultAsync();
        }
        public async Task<Wishlist> CreateAsync(Wishlist wishlist)
        {
            await _wishlist.InsertOneAsync(wishlist);
            return wishlist;
        }
        public async Task UpdateAsync(string id, Wishlist wishlistIn)
        {
            await _wishlist.ReplaceOneAsync(wishlist => wishlist.Id == id, wishlistIn);
        }
        public async Task RemoveAsync(Wishlist wishlistIn)
        {
            await _wishlist.DeleteOneAsync(wishlist => wishlist.Id == wishlistIn.Id);
        }
        public async Task RemoveAsync(string userId, string placeId)
        {
            await _wishlist.DeleteOneAsync(wishlist => wishlist.UserId == userId && wishlist.PlaceId == placeId);
        }
        public async Task RemoveByPlaceIdAsync(string placeId)
        {
            await _wishlist.DeleteManyAsync(wishlist => wishlist.PlaceId == placeId);
        }
    }
}
