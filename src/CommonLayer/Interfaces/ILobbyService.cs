namespace MyApp.CommonLayer.Interfaces;
using MyApp.CommonLayer.Models;

public interface ILobbyService
{
    Task<GameRoom> CreateRoomAsync();
    // Dodaj ostale metode koje će ti trebati
}