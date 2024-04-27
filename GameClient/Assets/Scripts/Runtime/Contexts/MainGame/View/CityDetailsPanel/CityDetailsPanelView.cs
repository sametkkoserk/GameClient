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

    [Header("Details")]
    [Space]
    public TextMeshProUGUI soldierCountText;
    
    [HideInInspector]
    public bool closing;

    public void OnDoOperation()
    {
      dispatcher.Dispatch(CityDetailsPanelEvent.OnDoOperation);
    }
  }
}