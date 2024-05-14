using Newtonsoft.Json;
using StackExchange.Redis;
using TravelAppAPI.Models.Interface;

namespace TravelAppAPI.Services
{
    public class CacheServices : ICacheService
    {
        private readonly IDatabase _db;
        public CacheServices()
        {
            _db = ConfigureRedis();
        }
        private static IDatabase ConfigureRedis()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
            return redis.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value!)!;
            }
            return default!;
        }
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }
        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }
    }
}

