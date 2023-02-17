using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;

namespace Lobby.View.LobbyManagerPanel
{
  public class LobbyManagerPanelView : EventView
  {
    public Transform playerContainer;
    public TMP_Text lobbyNameText;
    public TMP_Text playerCountText;
    public void OnReady()
    {
      dispatcher.Dispatch(LobbyManagerPanelEvent.Ready);
    }
    public void OnBack()
    {
      dispatcher.Dispatch(LobbyManagerPanelEvent.Back);
    }
  }
}