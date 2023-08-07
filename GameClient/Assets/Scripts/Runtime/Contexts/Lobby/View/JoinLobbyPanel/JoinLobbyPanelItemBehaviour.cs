using System;
using Runtime.Contexts.Lobby.Vo;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Runtime.Contexts.Lobby.View.JoinLobbyPanel
{
  public class JoinLobbyPanelItemBehaviour : MonoBehaviour
  {
    public TMP_Text lobbyName;

    public Button joinButton;

    public LobbyVo lobbyVo;

    public void Init(LobbyVo vo, UnityAction buttonAction)
    {
      lobbyVo = vo;
      
      lobbyName.text = $"{lobbyVo.lobbyName} ({lobbyVo.playerCount} / {lobbyVo.maxPlayerCount})";
      
      joinButton.onClick.AddListener(buttonAction);
    }
  }
}