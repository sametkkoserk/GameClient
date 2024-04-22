using EnhancedUI.EnhancedScroller;
using Runtime.Contexts.Lobby.Vo;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Runtime.Contexts.Lobby.View.JoinLobbyPanel
{
  public class JoinLobbyPanelItemBehaviour : EnhancedScrollerCellView
  {
    public TMP_Text lobbyName;

    public Button joinButton;

    public LobbyVo lobbyVo;

    public void SetData(LobbyVo vo, UnityAction buttonAction)
    {
      lobbyVo = vo;
      
      lobbyName.text = $"{lobbyVo.lobbyName} ({lobbyVo.playerCount} / {lobbyVo.maxPlayerCount})";
      
      joinButton.onClick.AddListener(buttonAction);
    }
  }
}