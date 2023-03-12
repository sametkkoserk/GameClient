using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Contexts.MainGame.View.YourTurnPanel
{
  public class YourTurnPanelView : EventView
  {
    public TextMeshProUGUI title;

    public Image sliderImage;

    public TextMeshProUGUI timer;

    [HideInInspector]
    public float totalTime;

    [HideInInspector]
    public float remainingTime;

    [HideInInspector]
    public bool turnEnded;
  }
}