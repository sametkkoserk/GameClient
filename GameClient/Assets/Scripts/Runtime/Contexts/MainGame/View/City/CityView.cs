using DG.Tweening;
using Runtime.Contexts.MainGame.Vo;
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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      Sequence scaleSequence = DOTween.Sequence();
      scaleSequence.Append(transform.DOScale(1.5f, 0.5f));

    }

    public void OnPointerExit(PointerEventData eventData)
    {
      Sequence scaleSequence = DOTween.Sequence();
      scaleSequence.Append(transform.DOScale(1f, 0.5f));
    }
  }
}