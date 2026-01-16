namespace MyApp.CommonLayer.Models
{
    public class Player
{
    // SignalR Connection ID je ključan za slanje poruka samo ovom igraču.
    public string ConnectionId { get; set; }

    // Jedinstveni ID korisnika iz vaše baze (ako imate login sistem).
    public string UserId { get; set; } 

    public string Username { get; set; }

    // Da li je ovaj igrač vlasnik (host) sobe?
    public bool IsHost { get; set; }

    // ----- Podaci specifični za igru -----
    
    // Da li je igrač Impostor u trenutnoj partiji?
    public bool IsImpostor { get; set; } = false;

    // Poeni osvojeni u trenutnoj partiji.
    public int Score { get; set; } = 0;

    // Da li je igrač spreman za sledeću rundu/igru?
    public bool IsReady { get; set; } = false;

    public Player(string connectionId, string userId, string username, bool isHost = false)
    {
        ConnectionId = connectionId;
        UserId = userId;
        Username = username;
        IsHost = isHost;
    }
}
}