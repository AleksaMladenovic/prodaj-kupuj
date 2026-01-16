namespace MyApp.Api.Hubs;

using Microsoft.AspNetCore.SignalR;
using MyApp.CommonLayer.Interfaces;
using MyApp.CommonLayer.Models;

public class GameHub : Hub
{
    private readonly IGameRoomRepository _gameRoomRepository;

    // Hub može direktno da koristi repozitorijum jer upravlja "živim" stanjem
    public GameHub(IGameRoomRepository gameRoomRepository)
    {
        _gameRoomRepository = gameRoomRepository;
    }

    public async Task JoinRoom(string roomId, string username)
    {
        var room = await _gameRoomRepository.GetByIdAsync(roomId);

        // Slučaj 1: Soba ne postoji
        if (room == null)
        {
            await Clients.Caller.SendAsync("Error", $"Soba sa ID-em '{roomId}' ne postoji.");
            return;
        }

        var connectionId = Context.ConnectionId;
        var player = new Player(connectionId, $"user_{Guid.NewGuid()}", username); // Privremeni User ID

        // Slučaj 2: Igrač je već u sobi (npr. refresh stranice)
        if (room.Players.ContainsKey(connectionId))
        {
            // Možemo samo da ga dodamo u grupu ponovo i pošaljemo mu trenutno stanje
            await Groups.AddToGroupAsync(connectionId, roomId);
            await Clients.Caller.SendAsync("PlayerListUpdated", room.Players.Values.ToList());
            return;
        }

        // Slučaj 3: Novi igrač se pridružuje
        room.Players.Add(connectionId, player);

        // Dodaj konekciju u SignalR grupu da prima poruke za ovu sobu
        await Groups.AddToGroupAsync(connectionId, roomId);

        // Sačuvaj ažurirano stanje sobe u Redis
        await _gameRoomRepository.SaveAsync(room);

        // Obavesti SVE igrače u sobi (uključujući novog) o promeni
        await Clients.Group(roomId).SendAsync("PlayerListUpdated", room.Players.Values.ToList());
    }

    // Opciono, ali preporučeno: Logika za izlazak iz sobe
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Ovde treba dodati logiku koja pronalazi sobu u kojoj je igrač bio,
        // uklanja ga iz liste, ažurira Redis i obaveštava ostale.
        // Za sada, ostavićemo je praznom radi jednostavnosti.
        await base.OnDisconnectedAsync(exception);
    }
}