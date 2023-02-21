using Runtime.MainGame.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.MainGame.View.City
{
  public class CityView : EventView
  {
    public CityVo cityVo;

    [HideInInspector]
    public Material material;

    public MeshRenderer meshRenderer;

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