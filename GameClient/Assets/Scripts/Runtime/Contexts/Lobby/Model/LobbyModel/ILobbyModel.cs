using Runtime.Contexts.Lobby.Vo;

namespace Runtime.Contexts.Lobby.Model.LobbyModel
{
  public interface ILobbyModel
  {
    LobbyVo lobbyVo { get; set; }

    ClientVo clientVo { get; set; }

    void SetLobbyVo(LobbyVo newLobbyVo);

    void SetClientVo(ClientVo newClientVo);

    void LobbyReset();
  }
}