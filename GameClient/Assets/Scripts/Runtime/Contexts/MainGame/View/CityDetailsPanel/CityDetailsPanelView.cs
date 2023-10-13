using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Contexts.MainGame.View.CityDetailsPanel
{
  public class CityDetailsPanelView : EventView
  {
    public GameObject buttons;
    
    public Button claimButton;

    public Button armingButton;

    [Header("Arming")]
    [Space]
    public GameObject armingPanel;

    public TextMeshProUGUI armingCountText;

    public Button increaseButton;

    public Button decreaseButton;
    
    [HideInInspector]
    public int armingCount;
      
    public void OnClosePanel()
    {
      dispatcher.Dispatch(CityDetailsPanelEvent.ClosePanel);
    }

    public void OnClaimCity()
    {
      dispatcher.Dispatch(CityDetailsPanelEvent.ClaimCity);
    }

    public void OnOpenArmingPanel()
    {
      dispatcher.Dispatch(CityDetailsPanelEvent.Arming);
    }

    public void OnArmingCountChange(bool value)
    {
      dispatcher.Dispatch(CityDetailsPanelEvent.ChangeArmingCount, value);
    }

    public void OnCloseArmingPanel()
    {
      dispatcher.Dispatch(CityDetailsPanelEvent.CloseArmingPanel);
    }

    public void OnConfirmArming()
    {
      dispatcher.Dispatch(CityDetailsPanelEvent.OnConfirmArming);
    }
  }
}