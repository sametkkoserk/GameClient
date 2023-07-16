using System;
using DG.Tweening;
using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Main.Vo;
using Runtime.Modules.Core.Localization.Enum;
using Runtime.Modules.Core.Localization.Model.LocalizationModel;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.Main.View.Tooltip
{
  public class TooltipMediator : EventMediator
  {
    [Inject]
    public TooltipView view { get; set; }

    [Inject]
    public ILocalizationModel localizationModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(TooltipEvent.Show, OnShow);
      dispatcher.AddListener(TooltipEvent.Hide, OnHide);

      FadeAnimation(0, 0);
    }

    private void Update()
    {
      view.layoutElement.enabled = Math.Max(view.headerField.preferredWidth, view.contentField.preferredWidth) >= view.layoutElement.preferredWidth;
    }
    
    private void OnShow(IEvent payload)
    {
      if (string.IsNullOrEmpty(view.headerField.text) || string.IsNullOrEmpty(view.contentField.text))
        return;

      TooltipInfoVo tooltipInfoVo = (TooltipInfoVo)payload.data;

      if (string.IsNullOrEmpty(tooltipInfoVo.headerKey.ToString()))
        view.headerField.gameObject.SetActive(false);
      if (string.IsNullOrEmpty(tooltipInfoVo.contentKey.ToString()))
        view.contentField.gameObject.SetActive(false);

      view.headerField.text = localizationModel.GetText(TableKey.Tooltip, tooltipInfoVo.headerKey);
      view.contentField.text = localizationModel.GetText(TableKey.Tooltip, tooltipInfoVo.contentKey);

      SetPosition(tooltipInfoVo.position);
      
      FadeAnimation(0.5f, 1);
    }

    private void SetPosition(Vector2 position)
    {
      float x = position.x / Screen.width;
      float y = position.y / Screen.height;
      
      switch (x)
      {
        case < 0.5f when y > 0.5f:
          //UpperLeft
          view.rectTransform.pivot = new Vector2(0, 1);
          break;
        case < 0.5f:
          //LowerLeft
          view.rectTransform.pivot = new Vector2(0, 0);
          break;
        case > 0.5f when y > 0.5f:
          //UpperRight
          view.rectTransform.pivot = new Vector2(1, 1);
          break;
        case > 0.5f:
          //LowerRight
          view.rectTransform.pivot = new Vector2(1, 0);
          break;
        default:
        {
          //Upper                                                      //Lower
          view.rectTransform.pivot = y > 0.5f ? new Vector2(0.5f, 1) : new Vector2(0.5f, 0);
          break;
        }
      }

      transform.position = position;
    }
    
    private void OnHide(IEvent payload)
    {
      float time = (float)payload.data;
      Hide(time);
    }

    private void Hide(float time)
    {
      FadeAnimation(time, 0);
    }
    
    private void FadeAnimation(float time, float fadeValue)
    {
      view.headerField.DOColor(new Color(view.headerField.color.r, view.headerField.color.g, view.headerField.color.b, fadeValue), time);
      view.contentField.DOColor(new Color(view.contentField.color.r, view.contentField.color.g, view.contentField.color.b, fadeValue), time);
      view.background.DOFade(fadeValue, time);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(TooltipEvent.Show, OnShow);
      dispatcher.RemoveListener(TooltipEvent.Hide, OnHide);
    }
  }
}