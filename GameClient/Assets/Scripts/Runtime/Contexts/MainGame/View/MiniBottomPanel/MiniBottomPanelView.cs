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

    public Image[] images;

    public TextMeshProUGUI playerName;

    public TextMeshProUGUI stateText;

    public Button operationButton;
    
    [Header("Fortify Part")]
    public List<GameObject> fortifyPart;
    
    public List<Button> changeCountButtons;

    public Button confirmButton;
    
    public TextMeshProUGUI soldierCountText;

    [HideInInspector]
    public int soldierCountInPanel;

    [HideInInspector]
    public int maxSoldierCount;

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