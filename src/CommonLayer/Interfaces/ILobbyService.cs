namespace MyApp.CommonLayer.Interfaces;

using CommonLayer.Models;
using global::CommonLayer.Models;

public interface ILobbyService
{
    Task<GameRoom> CreateRoomAsync();
    Task StartGameAsync(string roomId, int maxNumberOfRounds, int durationPerUserInSeconds);
    Task<GameRoom?> JoinRoomAsync(string roomId, string username, string userId, string connectionId);
    Task<GameRoom?> LeaveRoomAsync(string userId, string connectionId);
    Task<GameRoom?> RemovePlayerFromRoomAsync(string roomId, string userId, string connectionId);
    Task SendMessageToRoomAsync(string roomId, Message message);
    Task<List<Message>> GetMessagesFromRoomAsync(string roomId);

    Task SendClueToRoomAsync(string roomId, Clue clue);
    Task<List<Clue>> GetCluesFromRoomAsync(string roomId);
    Task<GameRoom?> AdvanceTurnAsync(string roomId);
    // Dodaj ostale metode koje će ti trebati
}