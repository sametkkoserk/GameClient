using Runtime.Contexts.MainGame.Model;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.City
{
  public enum CityEvent
  {
    OnClick
  }

  public class CityMediator : EventMediator
  {
    [Inject]
    public CityView view { get; set; }

    [Inject]
    public IMainGameModel mainGameModel { get; set; }

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
      view.cityVo.ownerID = newOwnerPlayerID;

      FillColor();
    }

    public void FillColor()
    {
      for (var i = 0; i < mainGameModel.materials.Count; i++)
      {
        if (i != view.cityVo.ownerID) continue;
        view.material = mainGameModel.materials[i];
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