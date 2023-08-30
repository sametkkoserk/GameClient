using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.CityDetailsPanel
{
  public class CityDetailsPanelView : EventView
  {
    public GameObject claimButton;
    public void OnClosePanel()
    {
      dispatcher.Dispatch(CityDetailsPanelEvent.ClosePanel);
    }

    public void OnClaimCity()
    {
      dispatcher.Dispatch(CityDetailsPanelEvent.ClaimCity);
    }
  }
}