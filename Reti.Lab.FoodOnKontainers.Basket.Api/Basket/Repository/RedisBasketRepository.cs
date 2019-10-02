using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace Reti.Lab.FoodOnKontainers.Basket.Api.Basket.Repository
{
    public class RedisBasketRepository : IBasketRepository
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisBasketRepository(ConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public IEnumerable<string> GetUsers()
        {
            var server = GetServer();
            var data = server.Keys();
            
            return data?.Select(k => k.ToString());
        }

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }
    }
}
