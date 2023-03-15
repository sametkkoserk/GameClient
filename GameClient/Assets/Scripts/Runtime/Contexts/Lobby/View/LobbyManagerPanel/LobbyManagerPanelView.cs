using System.Collections.Generic;
using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Runtime.Contexts.Lobby.View.LobbyManagerPanel
{
  public class LobbyManagerPanelView : EventView
  {
    public Dictionary<ushort, LobbyManagerPanelItemBehaviour> behaviours;

    public GameObject lobbyManagerPanelItem;
    
    public Transform playerContainer;
    
    public TMP_Text lobbyNameText;
    
    public TMP_Text playerCountText;

    public Button saveButton;
    
    public Button readyButton;

    [HideInInspector]
    public bool ready;

    [FormerlySerializedAs("anyChanges")]
    [HideInInspector]
    public bool changedSettings;

    [Header("Game Settings")]
    [Tooltip("Time of the turn.")]
    public TMP_Dropdown timerDropdown;
    
    [Tooltip("Only owner of lobby can change game settings.")]
    public List<GameObject> adminGameObjects;
    
    [Tooltip("Players will see these objects.")]
    public List<GameObject> playerGameObjects;

    public TextMeshProUGUI turnTimerText;

    public void OnReady()
    {
      dispatcher.Dispatch(LobbyManagerPanelEvent.Ready);
    }
    public void OnBack()
    {
      dispatcher.Dispatch(LobbyManagerPanelEvent.Back);
    }

    #region Game Settings

    public void OnSave()
    {
      dispatcher.Dispatch(LobbyManagerPanelEvent.Save);
    }

    public void OnTimerDropdownChanged()
    {
      dispatcher.Dispatch(LobbyManagerPanelEvent.ChangedSettings);
    }
    
    #endregion
  }
}