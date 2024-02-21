using System.Collections.Generic;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Contexts.MainGame.View.MiniBottomPanel
{
  public class MiniBottomPanelView : EventView
  {
    [Header("Mini Info Part")]
    public GameObject passButtonPart;

    public Image[] banners;

    public TextMeshProUGUI playerName;

    public TextMeshProUGUI stateText;

    public Button passButton;
    
    [Header("Soldier Selector Part")]
    public GameObject soldierSelector;
    
    public TextMeshProUGUI soldierCountText;

    [HideInInspector]
    public int soldierCountInPanel;

    [HideInInspector]
    public int maxSoldierCount;

    [HideInInspector]
    public int minSoldierCount;

    public KeyValuePair<int, int> cityIDs;

    public void OnPass()
    {
      dispatcher.Dispatch(MiniBottomPanelEvent.Pass);
    }

    public void OnIncrement()
    {
      dispatcher.Dispatch(MiniBottomPanelEvent.Increment);
    }
    
    public void OnDecrement()
    {
      dispatcher.Dispatch(MiniBottomPanelEvent.Decrement);
    }

    public void OnConfirm()
    {
      dispatcher.Dispatch(MiniBottomPanelEvent.Confirm);
    }
  }
}