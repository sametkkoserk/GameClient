using Runtime.Contexts.MainGame.Vo;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Contexts.MainGame.View.MainHudPanel.Item
{
  public class MainHudPanelPlayerItemView : EventView
  {
    public TextMeshProUGUI usernameText;

    public Image playerColorImage;

    public Transform background;

    [HideInInspector]
    public ushort id;
    
    public void Init(MainHudPlayerVo vo)
    {
      usernameText.text = vo.username;
      playerColorImage.color = vo.playerColor;
      id = vo.id;

      if (!vo.isMe)
        return;
      
      usernameText.color = Color.green;
      usernameText.fontStyle = FontStyles.Bold;
    }
  }
}