namespace MyApp.CommonLayer.Interfaces;
using MyApp.CommonLayer.Models;

public interface IGameRoomRepository
{
    Task<GameRoom?> GetByIdAsync(string roomId);
    Task SaveAsync(GameRoom room);
    Task DeleteAsync(string roomId);
}