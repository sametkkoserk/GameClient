using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Lobby.View.City
{
  public class CityView : EventView
  {
    [HideInInspector]
    public int soldierCount;

    [HideInInspector]
    public int ownerPlayerID; // Player ID in the room not general id.

    [HideInInspector]
    public Material material;

    public void OnClick()
    {
      dispatcher.Dispatch(CityEvent.OnClick);
    }
  }
}