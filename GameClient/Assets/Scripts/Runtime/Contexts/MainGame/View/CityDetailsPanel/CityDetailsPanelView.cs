using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Contexts.MainGame.View.CityDetailsPanel
{
  public class CityDetailsPanelView : EventView
  {
    [Header("Banner")]
    [Space]
    public TextMeshProUGUI ownerNameText;

    public Image ownerColorImage;
    
    [Header("Button")]
    [Space]
    public Button operationButton;

    public TextMeshProUGUI operationButtonText;

    [Header("Arming")]
    [Space]
    public GameObject armingPanel;

    public TextMeshProUGUI armingCountText;

    public Button increaseButton;

    public Button decreaseButton;

    [Header("Details")]
    [Space]
    public TextMeshProUGUI soldierCountText;
    
    [HideInInspector]
    public int armingCount;

    [HideInInspector]
    public bool closing;

    public void OnDoOperation()
    {
      dispatcher.Dispatch(CityDetailsPanelEvent.OnDoOperation);
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