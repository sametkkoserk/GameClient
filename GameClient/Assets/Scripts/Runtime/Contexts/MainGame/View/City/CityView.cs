using System.Collections.Generic;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Vo;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Contexts.MainGame.View.City
{
  public class CityView : EventView, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
  {
    public Material material;

    public MeshRenderer meshRenderer;

    public TextMeshProUGUI soldierCountText;

    public GameObject playableCityObjects;

    public CityVo cityVo;

    private bool isClickable = true;

    private CityModeKey _cityModeKey = CityModeKey.None;

    public void Init(CityVo _cityVo)
    {
      cityVo = _cityVo;

      transform.position = _cityVo.position.ToVector3();

      material = new Material(material)
      {
        color = cityVo.isPlayable ? Color.white : Color.gray
      };

      meshRenderer.material = material;
      
      dispatcher.Dispatch(CityEvent.OnUpdateCity);
    }

    public void ChangeOwner(CityVo _cityVo, Color color)
    {
      cityVo = _cityVo;

      material.color = color;
      meshRenderer.material = material;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
      if (!cityVo.isPlayable)
        return;
      
      dispatcher.Dispatch(CityEvent.OnClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      if (!cityVo.isPlayable)
        return;

      dispatcher.Dispatch(CityEvent.OnPointerEnter);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      if (!cityVo.isPlayable)
        return;
      
      dispatcher.Dispatch(CityEvent.OnPointerExit);
    }
    
    public bool GetClickable()
    {
      return isClickable;
    }

    public void SetClickable(bool value)
    {
      isClickable = value;
    }
    
    public CityModeKey GetCityModeKey()
    {
      return _cityModeKey;
    }

    public void SetCityModeKey(CityModeKey value)
    {
      _cityModeKey = value;
    }
  }
}