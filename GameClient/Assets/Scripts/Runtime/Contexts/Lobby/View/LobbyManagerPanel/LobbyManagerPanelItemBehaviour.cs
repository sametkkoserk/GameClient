using EnhancedUI.EnhancedScroller;
using Runtime.Contexts.Lobby.Vo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Contexts.Lobby.View.LobbyManagerPanel
{
  public class LobbyManagerPanelItemBehaviour : EnhancedScrollerCellView
  {
    public TMP_Text userNameText;

    public Image colorImage;

    public GameObject readyObj;
    
    public void SetData(ClientVo clientVo, bool isMe)
    {
      userNameText.text = clientVo.userName;
      colorImage.color = clientVo.playerColor.ToColor();

      readyObj.SetActive(clientVo.state==(ushort)ClientState.LobbyReady);
      
      userNameText.color = isMe ? Color.green : Color.white;
      userNameText.fontStyle = isMe ? FontStyles.Bold : FontStyles.Normal;
    }
  }
}