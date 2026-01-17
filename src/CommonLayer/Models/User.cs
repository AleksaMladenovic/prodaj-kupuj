using Cassandra.Mapping.Attributes;

namespace MyApp.CommonLayer.Models;

[Table("users")]
public class User
{
    [PartitionKey]
    [Column("user_id")]
    public string UserId { get; set; }

    [Column("username")]
    public string Username { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("games_played")]
    public long GamesPlayed { get; set; } = 0;

    [Column("wins_like_crewmate")]
    public long WinsLikeCrewmate { get; set; } = 0;

    [Column("wins_like_impostor")]
    public long WinsLikeImpostor { get; set; } = 0;

    [Column("total_score")]
    public long TotalScore { get; set; } = 0;

    public User(string userId, string username, string email)
    {
        UserId = userId;
        Username = username;
        Email = email;
    }

    public User()
    {
        UserId = string.Empty;
        Username = string.Empty;
        Email = string.Empty;
        GamesPlayed = 0;
        WinsLikeCrewmate = 0;
        WinsLikeImpostor = 0;
        TotalScore = 0;
    }
}