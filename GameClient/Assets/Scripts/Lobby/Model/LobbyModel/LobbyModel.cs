namespace Lobby.Model.LobbyModel
{
    public class LobbyModel : ILobbyModel
    {
        public ushort lobbyId { get; set; }
        public string lobbyName { get; set; }
        public bool isPrivate { get; set; }
        public ushort leaderId { get; set; }
        
        
    }
}