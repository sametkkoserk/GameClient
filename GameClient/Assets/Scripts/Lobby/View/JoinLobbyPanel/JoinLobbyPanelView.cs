using strange.extensions.mediation.impl;
using UnityEngine;

namespace Lobby.View.JoinLobbyPanel
{
  public class JoinLobbyPanelView : EventView
  {
    public Transform lobbyContainer;
    
    public void OnBack()
    {
      dispatcher.Dispatch(JoinLobbyPanelEvent.Back);
    }
  }
}