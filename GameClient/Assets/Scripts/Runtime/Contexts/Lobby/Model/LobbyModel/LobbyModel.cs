using Runtime.Contexts.Lobby.Vo;

namespace Runtime.Contexts.Lobby.Model.LobbyModel
{
  public class LobbyModel : ILobbyModel
  {
    public LobbyVo lobbyVo { get; set; }

    public ClientVo clientVo { get; set; }

    public void UpdateLobbyVo(LobbyVo newLobbyVo)
    {
      lobbyVo = newLobbyVo;
    }

    public void UpdateClientVo(ClientVo newClientVo)
    {
      clientVo = newClientVo;
    }
  }
}