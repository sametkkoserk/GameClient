using System.Collections.Generic;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.View.LobbyManagerPanel;
using Runtime.Contexts.Lobby.Vo;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Model.LobbyModel
{
  public class LobbyModel : ILobbyModel
  {
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

    public LobbyVo lobbyVo { get; set; }

    public ushort inLobbyId { get; set; }

    public ClientVo clientVo { get; set; }
    public List<Color> colors { get; set; }

    public Dictionary<ushort, LobbyManagerPanelItemBehaviour> userItemBehaviours { get; set; }

    public void OutFromLobby(ushort _inLobbyId)
    {
      if (lobbyVo.clients[_inLobbyId].ready)
        lobbyVo.readyCount -= 1;

      lobbyVo.playerCount -= 1;

      for (ushort i = _inLobbyId; i < lobbyVo.playerCount; i++)
      {
        lobbyVo.clients[i] = lobbyVo.clients[(ushort)(i + 1)];
        lobbyVo.clients[i].inLobbyId = i;
        userItemBehaviours[i] = userItemBehaviours[(ushort)(i + 1)];
        userItemBehaviours[i].inLobbyId = i;
        dispatcher.Dispatch(LobbyEvent.OnChangeUserLobbyID, lobbyVo.clients[i]);
      }

      userItemBehaviours.Remove(lobbyVo.playerCount);
      lobbyVo.clients.Remove(lobbyVo.playerCount);

      //
      // if (_inLobbyId < inLobbyId)
      // {
      //     inLobbyId -= 1;
      // }
    }

    public void LobbyReset()
    {
      lobbyVo = new LobbyVo();
      // inLobbyId = 0;
    }

    [PostConstruct]
    public void OnPostConstruct()
    {
      userItemBehaviours = new Dictionary<ushort, LobbyManagerPanelItemBehaviour>();
      colors = new List<Color> { Color.black, Color.blue, Color.green, Color.magenta, Color.red, Color.yellow };
    }
  }
}