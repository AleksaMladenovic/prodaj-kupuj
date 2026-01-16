namespace MyApp.CommonLayer.Enums;

public enum GameState
{
    Lobby,        // Igrači se okupljaju u lobiju
    InProgress,   // Igra je u toku
    Voting,       // Faza glasanja
    RoundFinished,// Kraj runde, prikaz rezultata
    GameFinished  // Kraj cele partije
}
