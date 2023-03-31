using System.Collections.Generic;
using Runtime.Contexts.Lobby.View.LobbyManagerPanel;
using Runtime.Contexts.Lobby.Vo;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Model.LobbyModel
{
  public interface ILobbyModel
  {
    LobbyVo lobbyVo { get; set; }

    ushort inLobbyId { get; set; }

    ClientVo clientVo { get; set; }

    List<Color> colors { get; set; }

    Dictionary<ushort, LobbyManagerPanelItemBehaviour> userItemBehaviours { get; set; }

    void OutFromLobby(ushort _inLobbyId);

    void LobbyReset();
  }
}