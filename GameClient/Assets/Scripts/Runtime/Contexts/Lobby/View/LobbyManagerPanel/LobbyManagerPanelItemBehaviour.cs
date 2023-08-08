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

      if (clientVo.ready)
        readyObj.SetActive(true);
      
      if (!isMe) return;
      userNameText.color = Color.green;
      userNameText.fontStyle = FontStyles.Bold;    
    }
  }
}