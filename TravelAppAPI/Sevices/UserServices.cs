using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using TravelAppAPI.Models;

namespace TravelAppAPI.Sevices
{
    public class UserServices
    {
        private readonly IMongoCollection<User> _users;
        public UserServices(IOptions<TravelAppDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>(settings.Value.UsersCollectionName);
        }
        public async Task<List<User>> GetAsync()
        {
            return await _users.Find(user => true).ToListAsync();
        }
        public async Task<User> GetAsync(string id)
        {
            User user = await _users.Find<User>(user => user.Id == id).FirstOrDefaultAsync();
            user.Password = String.Empty;
            return user;
        }
        public async Task<string> GetUserPassword(string username)
        {
            User user = await _users.Find<User>(user => user.Username == username).FirstOrDefaultAsync();
            return user.Password;
        }
        public string DecodeJwtToken(HttpRequest request)
        {
            String authHeader = request.Headers.Authorization!;
            authHeader = authHeader.Replace("Bearer ", String.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            string userId = tokenS!.Claims.First(claim => claim.Type == "Id").Value;
            return userId;
        }
        public async Task UpdateAsync(string id, User userIn)
        {
            await _users.ReplaceOneAsync(user => user.Id == id, userIn);
        }
        public async Task RemoveAsync(User userIn)
        {
            await _users.DeleteOneAsync(user => user.Id == userIn.Id);
        }
        public async Task RemoveAsync(string id)
        {
            await _users.DeleteOneAsync(user => user.Id == id);
        }



    }
}
