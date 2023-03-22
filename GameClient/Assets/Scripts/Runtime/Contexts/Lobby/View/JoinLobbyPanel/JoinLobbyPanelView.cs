using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.Lobby.View.JoinLobbyPanel
{
  public class JoinLobbyPanelView : EventView
  {
    public GameObject joinLobbyPanelItem;
    
    public Transform lobbyContainer;
    
    public void OnBack()
    {
      dispatcher.Dispatch(JoinLobbyPanelEvent.Back);
    }
  }
}