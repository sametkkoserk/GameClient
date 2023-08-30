using System.Collections.Generic;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Vo;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Contexts.MainGame.View.City
{
  public class CityView : EventView, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
  {
    public Material material;

    public MeshRenderer meshRenderer;

    public CityVo cityVo;

    public PlayerActionKey playerActionKey;

    [HideInInspector]
    public List<GameStateKey> necessaryGameStateKeyForOpenDetailsPanel;
    
    [HideInInspector]
    public List<PlayerActionKey> necessaryPlayerActionKeysForOpenDetailsPanel;

    public void Init(CityVo _cityVo)
    {
      cityVo = _cityVo;

      transform.position = _cityVo.position.ToVector3();

      material = new Material(material);
      
      if (cityVo.isPlayable) return;
      material.color = Color.gray;
      meshRenderer.material = material;
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
  }
}