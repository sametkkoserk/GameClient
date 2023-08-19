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
    [HideInInspector]
    public Material material;

    public MeshRenderer meshRenderer;
    
    public CityVo cityVo;

    public void Init(CityVo _cityVo)
    {
      cityVo = _cityVo;

      transform.position =  _cityVo.position.ToVector3();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
      dispatcher.Dispatch(CityEvent.OnClick);
      
      transform.DOScale(1f, 0.5f);
      
      CursorModel.instance.OnChangeCursor(CursorKey.Default);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      transform.DOScale(1.25f, 0.5f);
      
      CursorModel.instance.OnChangeCursor(CursorKey.Click);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      transform.DOScale(1f, 0.5f);
      
      CursorModel.instance.OnChangeCursor(CursorKey.Default);
    }
  }
}