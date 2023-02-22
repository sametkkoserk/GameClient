using System.Collections.Generic;
using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Contexts.Lobby.View.LobbyManagerPanel
{
  public class LobbyManagerPanelView : EventView
  {
    public Dictionary<ushort, LobbyManagerPanelItemBehaviour> behaviours;

    public Transform playerContainer;
    public TMP_Text lobbyNameText;
    public TMP_Text playerCountText;
    public Button readyButton;
    public void OnReady()
    {
      readyButton.interactable = false;
      dispatcher.Dispatch(LobbyManagerPanelEvent.Ready);
    }
    public void OnBack()
    {
      dispatcher.Dispatch(LobbyManagerPanelEvent.Back);
    }
  }
}