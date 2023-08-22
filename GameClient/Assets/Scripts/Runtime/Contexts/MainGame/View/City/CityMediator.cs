using Runtime.Contexts.MainGame.Enum;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.City
{
  public enum CityEvent
  {
    OnClick,
    OnPointerEnter,
    OnPointerExit
  }

  public class CityMediator : EventMediator
  {
    [Inject]
    public CityView view { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(CityEvent.OnClick, OnClick);
      view.dispatcher.AddListener(CityEvent.OnPointerEnter, OnPointerEnter);
      view.dispatcher.AddListener(CityEvent.OnPointerExit, OnPointerExit);
    }

    public void OnClick()
    {
      Debug.Log("Clicked");
    }

    private void OnPointerEnter()
    {
      dispatcher.Dispatch(MainGameEvent.ShowCityMiniInfoPanel, view.cityVo);
    }
    
    private void OnPointerExit()
    {
      dispatcher.Dispatch(MainGameEvent.HideCityMiniInfoPanel);
    }
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(CityEvent.OnClick, OnClick);
      view.dispatcher.RemoveListener(CityEvent.OnPointerEnter, OnPointerEnter);
      view.dispatcher.RemoveListener(CityEvent.OnPointerExit, OnPointerExit);
    }
  }
}