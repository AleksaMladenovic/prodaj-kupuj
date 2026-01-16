using MyApp.CommonLayer.Interfaces;
using MyApp.CommonLayer.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace MyApp.DatabaseLayer.Repositories;


public class RedisGameRoomRepository : IGameRoomRepository
{
    private readonly IDatabase _redisDb;

    public RedisGameRoomRepository(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    public async Task<GameRoom?> GetByIdAsync(string roomId)
    {
        var roomJson = await _redisDb.StringGetAsync($"room:{roomId}");
        return roomJson.IsNullOrEmpty ? null : JsonSerializer.Deserialize<GameRoom>(roomJson!);
    }

    public async Task SaveAsync(GameRoom room)
    {
        var roomJson = JsonSerializer.Serialize(room);
        await _redisDb.StringSetAsync($"room:{room.RoomId}", roomJson);
    }
    
    public async Task DeleteAsync(string roomId)
    {
        await _redisDb.KeyDeleteAsync($"room:{roomId}");
    }
}