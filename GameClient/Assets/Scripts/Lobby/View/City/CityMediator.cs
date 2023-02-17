using Lobby.Model.LobbyModel;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Lobby.View.City
{
  public enum CityEvent
  {
    OnClick,
  }
  public class CityMediator : EventMediator
  {
    [Inject]
    public CityView view { get; set; }
    
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(CityEvent.OnClick, OnClick);
    }

    public void OnClick()
    {
      Debug.Log("Clicked");
    }

    public void Conquer(int newOwnerPlayerID)
    {
      view.ownerPlayerID = newOwnerPlayerID;
      
      FillColor();
    }

    public void FillColor()
    {
      for (int i = 0; i < lobbyModel.materials.Count; i++)
      {
        if (i != view.ownerPlayerID) continue;
        view.material = lobbyModel.materials[i];
        return;
      }

      view.meshRenderer.material = view.material;
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(CityEvent.OnClick, OnClick);
    }
  }
}