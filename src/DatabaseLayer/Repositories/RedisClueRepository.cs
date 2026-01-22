using CommonLayer.Interfaces;
using CommonLayer.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Repositories
{
    public class RedisClueRepository : IClueRepository
    {
        private readonly IDatabase _redisDb;

        public RedisClueRepository(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        public Task AddClueToRoomAsync(string roomId, Clue clue)
        {
            try
            {
                string key = $"clues:{roomId}";

                string serializedClue = System.Text.Json.JsonSerializer.Serialize(clue);

                _redisDb.ListRightPush(key, serializedClue);
                return Task.CompletedTask;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Error serializing clue: {ex.Message}");
                throw;
            }
        }

        public Task<List<Clue>> GetCluesFromRoomAsync(string roomId)
        {
            try
            {
                string key = $"clues:{roomId}";
                var clueRedis = _redisDb.ListRange(key);
                var clues= clueRedis.Select(clue=> System.Text.Json.JsonSerializer.Deserialize<Clue>(clue))
                                    .Where(clue=>clue!=null)
                                    .ToList();
                return Task.FromResult(clues) as Task<List<Clue>>;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing clues: {ex.Message}");
                throw;
            }
        }
    }
}
