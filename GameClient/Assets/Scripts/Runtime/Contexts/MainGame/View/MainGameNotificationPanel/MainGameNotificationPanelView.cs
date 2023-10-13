using System.Collections.Generic;
using Runtime.Contexts.MainGame.Vo;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Contexts.MainGame.View.MainGameNotificationPanel
{
  public class MainGameNotificationPanelView : EventView
  {
    public TextMeshProUGUI title;

    public List<Image> colorImages;

    public CanvasGroup canvasGroup;

    public MainHudTurnVo notificationPanelVo;
  }
}