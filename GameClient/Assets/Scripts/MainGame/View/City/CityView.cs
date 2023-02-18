using strange.extensions.mediation.impl;
using UnityEngine;

namespace MainGame.View.City
{
  public class CityView : EventView
  {
    [HideInInspector]
    public int soldierCount;

    [HideInInspector]
    public int ownerPlayerID; // Player ID in the room not general id.

    [HideInInspector]
    public Material material;

    public MeshRenderer meshRenderer;

    public void OnClick()
    {
      dispatcher.Dispatch(CityEvent.OnClick);
    }
  }
}