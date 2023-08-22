using DG.Tweening;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Modules.Core.Cursor.Enum;
using Runtime.Modules.Core.Cursor.Model.CursorModel;
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

    public void Init(CityVo _cityVo)
    {
      cityVo = _cityVo;

      transform.position = _cityVo.position.ToVector3();

      material = new Material(material);
      
      if (cityVo.isPlayable) return;
      material.color = Color.gray;
      meshRenderer.material = material;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
      if (!cityVo.isPlayable)
        return;
      
      dispatcher.Dispatch(CityEvent.OnClick);
      
      transform.DOMoveY(0f, 0.5f);
      
      CursorModel.instance.OnChangeCursor(CursorKey.Default);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      if (!cityVo.isPlayable)
        return;

      dispatcher.Dispatch(CityEvent.OnPointerEnter);

      transform.DOMoveY(0.25f, 0.5f);
      
      CursorModel.instance.OnChangeCursor(CursorKey.Click);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      if (!cityVo.isPlayable)
        return;
      
      dispatcher.Dispatch(CityEvent.OnPointerExit);

      transform.DOMoveY(0f, 0.5f);
      
      CursorModel.instance.OnChangeCursor(CursorKey.Default);
    }
  }
}