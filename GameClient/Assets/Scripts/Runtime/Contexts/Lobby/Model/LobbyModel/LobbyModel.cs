using Runtime.Contexts.Lobby.Vo;

namespace Runtime.Contexts.Lobby.Model.LobbyModel
{
  public class LobbyModel : ILobbyModel
  {
    public LobbyVo lobbyVo { get; set; }

    public ClientVo clientVo { get; set; }

    public void SetLobbyVo(LobbyVo newLobbyVo)
    {
      lobbyVo = newLobbyVo;
    }

    public void SetClientVo(ClientVo newClientVo)
    {
      clientVo = newClientVo;
    }

    public void LobbyReset()
    {
      lobbyVo = new LobbyVo();
    }
  }
}