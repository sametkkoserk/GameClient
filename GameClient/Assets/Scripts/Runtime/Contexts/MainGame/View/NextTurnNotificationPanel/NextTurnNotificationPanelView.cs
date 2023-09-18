using Runtime.Contexts.MainGame.Vo;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.NextTurnNotificationPanel
{
  public class NextTurnNotificationPanelView : EventView
  {
    public TextMeshProUGUI nameOfTurnOwner;

    public CanvasGroup canvasGroup;

    public MainHudTurnVo notificationPanelVo;
  }
}