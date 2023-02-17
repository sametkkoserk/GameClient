namespace Lobby.Model.LobbyModel
{
    public interface ILobbyModel
    {
        ushort lobbyId { get; set; }
        string lobbyName { get; set; }
        bool isPrivate { get; set; }
        ushort leaderId { get; set; }
    }
}