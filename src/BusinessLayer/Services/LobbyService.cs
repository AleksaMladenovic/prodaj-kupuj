using Cassandra.Serialization;
using CommonLayer.Interfaces;
using CommonLayer.Models;
using MyApp.CommonLayer.Enums;
using MyApp.CommonLayer.Interfaces;
using MyApp.CommonLayer.Models;

namespace MyApp.BusinessLayer.Services;


public class LobbyService : ILobbyService
{
    private readonly IGameRoomRepository _gameRoomRepository;
    private readonly ISecretWordService _secretWordService;
    private readonly IUserService _userService;
    private readonly IChatRepository _chatRepository;

    private readonly IClueRepository _clueRepository;
    // Zavisimo od interfejsa, ne od konkretne Redis implementacije!
    public LobbyService(IGameRoomRepository gameRoomRepository, ISecretWordService secretWordService, IChatRepository chatRepository,IClueRepository clueRepository,IUserService user)
    {
        _gameRoomRepository = gameRoomRepository;
        _secretWordService = secretWordService;
        _chatRepository = chatRepository;
        _clueRepository = clueRepository;
        _userService = user;
    }

    public async Task<GameRoom> CreateRoomAsync()
    {
        var roomId = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        var room = new GameRoom(roomId);

        await _gameRoomRepository.SaveAsync(room);

        return room;
    }

    public async Task StartGameAsync(string roomId, int maxNumberOfRounds, int durationPerUserInSeconds)
    {
        var room = await _gameRoomRepository.GetByIdAsync(roomId);
        if (room == null)
        {
            throw new Exception("Room not found");
        }

        room.NumberOfRounds = maxNumberOfRounds;
        room.SecondsPerTurn = durationPerUserInSeconds;
        room.State = GameState.InProgress;
        room.CurrentRound = 1;

        room.SecretWord = await _secretWordService.GetRandomSecretWordAsync();

        // Izaberi nasumičnog igrača za prvi potez
        var playerIds = room.Players.Keys.ToList();

        room.CurrentTurnPlayerId = playerIds[Random.Shared.Next(playerIds.Count)];
        room.CurrentTurnPlayerUsername = _userService.GetUserById(room.CurrentTurnPlayerId).Username;

        room.UserIdOfImpostor = playerIds[Random.Shared.Next(playerIds.Count)];

        room.SubmittedClues.Clear();
        room.Votes.Clear();

        await _gameRoomRepository.SaveAsync(room);

    }

    public async Task<GameRoom?> JoinRoomAsync(string roomId, string username, string userId, string connectionId)
    {
        var room = await _gameRoomRepository.GetByIdAsync(roomId);
        if (room == null)
            return null;

        await _gameRoomRepository.RemoveTimerForRoom(roomId);
        var player = new Player(connectionId, userId, username, room.Players.Count == 0);

        if (room.Players.ContainsKey(userId))
        {
            await _gameRoomRepository.SaveUserIdForConnection(userId, connectionId);
            return room;
        }

        room.Players.Add(userId, player);
        await _gameRoomRepository.SaveRoomForUserId(userId, roomId);
        await _gameRoomRepository.SaveUserIdForConnection(userId, connectionId);
        await _gameRoomRepository.SaveAsync(room);
        return room;
    }

    public async Task<GameRoom?> LeaveRoomAsync(string userId, string connectionId)
    {
        var roomId = await _gameRoomRepository.GetRoomFromUserId(userId);
        if (string.IsNullOrEmpty(roomId))
            return null;
        return await RemovePlayerFromRoomAsync(roomId, userId, connectionId);
    }

    public async Task<GameRoom?> RemovePlayerFromRoomAsync(string roomId, string userId, string connectionId)
    {
        var room = await _gameRoomRepository.GetByIdAsync(roomId);
        if (room == null)
            return null;
        if (room.Players.ContainsKey(userId))
        {
            if (room.Players[userId].IsHost && room.Players.Count > 1)
            {
                var newHost = room.Players.Values.First(p => p.UserId != userId);
                newHost.IsHost = true;
            }
            room.Players.Remove(userId);
            await _gameRoomRepository.RemoveRoomForUserId(userId);
            await _gameRoomRepository.RemoveUserIdForConnection(connectionId);
            await _gameRoomRepository.SaveAsync(room);
            if (room.Players.Count == 0)
            {
                await _gameRoomRepository.DeleteAsync(roomId, 30);
            }
        }
        return room;
    }

    public async Task SendMessageToRoomAsync(string roomId, Message message)
    {
        try
        {
            await _chatRepository.AddMessageToRoomAsync(roomId, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message to room: {ex.Message}");
            throw;
        }

    }

    public async Task<List<Message>> GetMessagesFromRoomAsync(string roomId)
    {
        try
        {
            return await _chatRepository.GetMessagesFromRoomAsync(roomId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting messages from room: {ex.Message}");
            throw;
        }
    }

    public async Task SendClueToRoomAsync(string roomId, Clue clue)
    {
        try
        {
            await _clueRepository.AddClueToRoomAsync(roomId, clue);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending clue to room: {ex.Message}");
            throw;
        }
    }

    public async Task<List<Clue>> GetCluesFromRoomAsync(string roomId)
    {
        try
        {
            return await _clueRepository.GetCluesFromRoomAsync(roomId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting clues from room: {ex.Message}");
            throw;
        }
    }

    public async Task<GameRoom?> AdvanceTurnAsync(string roomId)
    {
        var room = await _gameRoomRepository.GetByIdAsync(roomId);

        if (room == null || room.Players == null || room.Players.Count == 0)
            return null;

        var sortedPlayers = room.Players.Values.OrderBy(p => p.UserId).ToList();

        int currentIndex = sortedPlayers.FindIndex(p => p.Username == room.CurrentTurnPlayerUsername);

        if (currentIndex == -1) currentIndex = 0;

        int nextIndex = (currentIndex + 1) % sortedPlayers.Count;

        room.CurrentTurnPlayerUsername = sortedPlayers[nextIndex].Username;

        if (nextIndex == 0)
        {
            room.CurrentRound++;
        }

        await _gameRoomRepository.SaveAsync(room);

        return room;
    }
}