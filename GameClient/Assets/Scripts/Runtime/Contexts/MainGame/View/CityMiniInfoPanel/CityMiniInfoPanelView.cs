using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Contexts.MainGame.View.CityMiniInfoPanel
{
  public class CityMiniInfoPanelView : EventView
  {
    public Image ownerColorImage;

    public TextMeshProUGUI ownerNameText;

    [Header("Item 1")]
    public TextMeshProUGUI itemOneText;
  }
}