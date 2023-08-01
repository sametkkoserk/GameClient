using Runtime.Contexts.Lobby.Vo;

namespace Runtime.Contexts.Lobby.Model.LobbyModel
{
  public interface ILobbyModel
  {
    LobbyVo lobbyVo { get; set; }

    ClientVo clientVo { get; set; }

    void UpdateLobbyVo(LobbyVo newLobbyVo);

    void UpdateClientVo(ClientVo newClientVo);
  }
}