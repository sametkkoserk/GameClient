using Runtime.Contexts.MainGame.Vo;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.City
{
  public class CityView : EventView
  {
    [HideInInspector]
    public Material material;

    public MeshRenderer meshRenderer;
    public CityVo cityVo;

    public void OnClick()
    {
      dispatcher.Dispatch(CityEvent.OnClick);
    }

    public void Init(CityVo _cityVo)
    {
      cityVo = _cityVo;

      transform.position = _cityVo.position;
    }
  }
}